using System.Text;
using AutoMapper;
using ESourcing.Order.Application.Features.Orders.Commands.Create;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events.Concretes;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ESourcing.Order.Consumers;

public class EventBusOrderCreateConsumer(
    IRabbitMQPersistentConnection persistentConnection,
    IMediator mediator,
    IMapper mapper)
{
    public void Consume()
    {
        if (!persistentConnection.IsConnected)
            persistentConnection.TryConnect();

        var channel = persistentConnection.CreateModel();
        channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: false, exclusive: false,
            autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += ReceivedEvent!;

        channel.BasicConsume(queue: EventBusConstants.OrderCreateQueue, autoAck: true, consumer: consumer);
    }

    private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
    {
        var message = Encoding.UTF8.GetString(e.Body.Span);
        var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message);

        if (e.RoutingKey != EventBusConstants.OrderCreateQueue) return;
        var command = mapper.Map<OrderCreateCommand>(@event);

        command.CreatedAt = DateTime.Now;
        if (@event != null)
        {
            command.TotalPrice = @event.Quantity * @event.Price;
            command.UnitPrice = @event.Price;
        }

        mediator.Send(command).Wait();
    }

    public void Disconnect()
    {
        persistentConnection.Dispose();
    }
}