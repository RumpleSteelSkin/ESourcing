using System.Net.Sockets;
using System.Text;
using EventBusRabbitMQ.Events.Abstracts;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace EventBusRabbitMQ.Producer;

public class EventBusRabbitMQProducer(
    IRabbitMQPersistentConnection persistentConnection,
    ILogger<EventBusRabbitMQProducer> logger,
    int retryCount = 5)
{
    private readonly IRabbitMQPersistentConnection _persistentConnection =
        persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));

    private readonly ILogger<EventBusRabbitMQProducer> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly int _retryCount = retryCount > 0 ? retryCount : 5;

    public void Publish(string queueName, EventBase @event)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var policy = Policy
            .Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.LogError(ex,
                        "RabbitMQ publish failed. Retrying in {Delay}s. Exception: {Message}",
                        time.TotalSeconds, ex.Message);
                });

        policy.Execute(() =>
        {
            using var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.ConfirmSelect();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                mandatory: true,
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)));

            if (!channel.WaitForConfirms(TimeSpan.FromSeconds(5)))
            {
                throw new Exception("RabbitMQ did not confirm message publish.");
            }
        });

        _logger.LogInformation("Event published to queue {Queue}. Event: {EventType}", queueName,
            @event.GetType().Name);
    }
}