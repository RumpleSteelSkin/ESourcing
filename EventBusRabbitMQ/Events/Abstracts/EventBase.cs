namespace EventBusRabbitMQ.Events.Abstracts;

public abstract class EventBase
{
    public Guid RequestId { get; private init; } = Guid.NewGuid();
    public DateTime CreationDate { get; private init; } = DateTime.Now;
}