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
    IServiceScopeFactory serviceScopeFactory,
    IMapper mapper,
    ILogger<EventBusOrderCreateConsumer> logger)
{
    public void Consume()
    {
        if (!persistentConnection.IsConnected)
            persistentConnection.TryConnect();
        var channel = persistentConnection.CreateModel();
        channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: true, exclusive: false,
            autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var orderEvent = JsonConvert.DeserializeObject<OrderCreateEvent>(Encoding.UTF8.GetString(ea.Body.Span));
                if (orderEvent is null)
                {
                    logger.LogError("EventBusOrderCreateConsumer received null event");
                    return;
                }

                using var scope = serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var command = mapper.Map<OrderCreateCommand>(orderEvent);
                command.CreatedAt = DateTime.Now;
                command.TotalPrice = orderEvent.Quantity * orderEvent.Price;
                command.UnitPrice = orderEvent.Price;
                await mediator.Send(command);
                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EventBusOrderCreateConsumer received exception");
                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };
        channel.BasicConsume(queue: EventBusConstants.OrderCreateQueue, autoAck: false, consumer: consumer);
    }

    public void Disconnect()
    {
        persistentConnection.Dispose();
    }
}