using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared;

public class DefaultAmqpBroker(
    ConnectionFactory factory,
    BrokerConfig brokerConfig,
    ILogger<IAmqpBroker> logger
) : IAmqpBroker
{
    private IChannel? _channel;
    private readonly ConnectionFactory _factory = factory;
    private readonly Exchange[] _exchanges = brokerConfig.Exchanges;
    private readonly IList<IAmqpMessageProcessor> _messageProcessors = [];
    private readonly ILogger<IAmqpBroker> _logger = logger;

    public async Task Connect()
    {
        IConnection conn = await _factory.CreateConnectionAsync();
        _channel = await conn.CreateChannelAsync();
        _exchanges
            .ToList()
            .ForEach(async exchange => await _channel.ExchangeDeclareAsync(exchange.Name, exchange.Type));
        _logger.LogInformation("Connected to AMQP broker with host {Host}.", _factory.Uri);
    }

    public async Task ConsumeFromTopic(ConsumerConfig config)
    {
        EnsureValidExchange(config.ExchangeName);

        string queueName = (await _channel!.QueueDeclareAsync()).QueueName;

        await _channel!.QueueBindAsync(
            queueName,
            config.ExchangeName,
            config.Event
        );

        AsyncEventingBasicConsumer consumer = new(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

            ConsumerContext ctx = CreateConsumerContext(config, message);

            if (ea.BasicProperties.IsContentTypePresent())
                ctx.ContentType = ea.BasicProperties.ContentType;

            foreach (var processor in _messageProcessors)
                await processor.ProcessMessage(ctx);

            await Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer);
    }

    public async Task PublishOnTopic(string exchangeName, string routingKey, string message)
    {
        EnsureValidExchange(exchangeName);

        BasicProperties? props = new()
        {
            ContentType = "application/json"
        };

        await _channel!.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: System.Text.Encoding.UTF8.GetBytes(message)
        );

        _logger.LogInformation("Published message: {Body}, with routing key: {RoutingKey}", message, routingKey);
    }

    public IAmqpBroker AddMessageProcessor(IAmqpMessageProcessor messageProcessor)
    {
        _messageProcessors.Add(messageProcessor);
        return this;
    }

    private void EnsureValidExchange(string exchangeName)
    {
        if (_exchanges.All(exchange => exchange.Name != exchangeName))
            throw new ArgumentException($"Exchange {exchangeName} is not valid.");
    }

    private static ConsumerContext CreateConsumerContext(ConsumerConfig config, string message)
    {
        return new(config.ExchangeName, config.Event, config.OperationId, message, null);
    }
}