using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared;

public sealed class AmqpBrokerConfigurator
{
    private readonly string _host;
    private readonly IList<ConsumerConfig> _consumerConfigs;
    private readonly IList<PublisherConfig> _publisherConfigs;
    private readonly BrokerConfig _brokerConfig;
    private readonly ILogger<AmqpBrokerConfigurator> _logger;

    private AmqpBrokerConfigurator(
        ILogger<AmqpBrokerConfigurator> logger,
        string host,
        IList<ConsumerConfig> consumerConfigs,
        IList<PublisherConfig> publisherConfigs,
        BrokerConfig brokerConfig
    )
    {
        _host = host;
        _consumerConfigs = consumerConfigs;
        _publisherConfigs = publisherConfigs;
        _brokerConfig = brokerConfig;
        _logger = logger;
    }

    public static AmqpBrokerConfigurator Create(
        ILogger<AmqpBrokerConfigurator> logger,
        string asyncApiSpecification,
        string hostname,
        string port,
        string virtualHost,
        string username,
        string password
    )
    {
        var host = $"amqp://{username}:{password}@{hostname}:{port}/{virtualHost}";
        logger.LogInformation("Connecting to AMQP broker at {Host}.", host);
        var asyncApiYaml = File.ReadAllText(asyncApiSpecification);
        var (brokerConfig, publishers, consumers) = AsyncApiParser.ParseAsyncApi(asyncApiYaml);
        return new AmqpBrokerConfigurator(logger, host, consumers, publishers, brokerConfig);
    }

    public IAmqpBroker CreateMessageBroker(ILogger<IAmqpBroker> logger)
    {
        ConnectionFactory connectionFactory = new()
        {
            Uri = new Uri(_host)
        };

        return new DefaultAmqpBroker(connectionFactory, _brokerConfig, logger);
    }

    public async Task RegisterAmqpTopicConsumersAsync(IAmqpBroker amqpBroker)
    {
        foreach (var config in _consumerConfigs)
        {
            await amqpBroker.ConsumeFromTopic(config);

            _logger.LogInformation(
                "Registered consumer on exchange '{ExchangeName}' " +
                "for event '{Event}' with operationId '{OperationId}'.",
                config.ExchangeName,
                config.Event,
                config.OperationId
            );
        }
    }

    public IEnumerable<AmqpTopicPublisher> RegisterAmqpTopicPublishers(IAmqpBroker amqpBroker)
    {
        foreach (var pc in _publisherConfigs)
        {
            _logger.LogInformation(
                "Registered publisher on exchange '{ExchangeName}' " +
                "for events '{Events}'.",
                pc.ExchangeName,
                string.Join(", ", pc.Events)
            );
        }

        return _publisherConfigs
            .Select(pc => new AmqpTopicPublisher(amqpBroker, pc.ExchangeName, pc.Events))
            .ToList();
    }
}