using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using EventBusRabbitMQ.Events.Abstracts;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer(
        IRabbitMQPersistentConnection persistentConnection,
        ILogger<EventBusRabbitMQProducer> logger,
        int retryCount = 5)
    {
        public void Publish(string queueName, EventBase @event)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(
                    retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        logger.LogWarning(ex,
                            "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})",
                            @event.RequestId, $"{time.TotalSeconds:n1}", ex.Message);
                    })
                .Execute(() =>
                {
                    using var channel = persistentConnection.CreateModel();
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    channel.ConfirmSelect();
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);

                    if (!channel.WaitForConfirms(TimeSpan.FromSeconds(5)))
                        throw new Exception($"RabbitMQ did not confirm publish for event {@event.RequestId}");
                    logger.LogInformation("Event {EventId} published to {Queue}", @event.RequestId, queueName);
                });
        }
    }
}