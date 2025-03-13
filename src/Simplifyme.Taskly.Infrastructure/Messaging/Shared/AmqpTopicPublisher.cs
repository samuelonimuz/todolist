using Domaincrafters.Domain;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Messages;

namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared;

public sealed class AmqpTopicPublisher(
    IAmqpBroker amqpBroker,
    string exchangeName,
    IList<string> allowedTopics,
    string? contentType = "application/json"
) : IDomainEventSubscriber
{
    private readonly IAmqpBroker _amqpBroker = amqpBroker;
    private readonly string _exchangeName = exchangeName;
    private readonly IList<string> _allowedTopics = allowedTopics;
    private readonly string? _contentType = contentType;

    public void HandleEvent(IDomainEvent domainEvent)
    {
        string routingKey = RoutingKey(domainEvent);

        if (IsSubscribedTo(domainEvent))
            _amqpBroker.PublishOnTopic(
                _exchangeName,
                routingKey,
                 AmqpMessageConverter.Serialize(domainEvent, _contentType)
            );
    }

    public bool IsSubscribedTo(IDomainEvent domainEvent)
    {
        return _allowedTopics.Contains(RoutingKey(domainEvent));
    }

    private string RoutingKey(IDomainEvent domainEvent)
    {
        return $"{_exchangeName}.{domainEvent.QualifiedEventName}";
    }
}
