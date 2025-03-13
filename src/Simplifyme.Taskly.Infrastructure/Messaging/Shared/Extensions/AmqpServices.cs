using Domaincrafters.Domain;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared.Extensions;

public static class AmqpServices
{
    public static IServiceCollection AddAmqpServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services
            .AddAmqpBrokerConfigurator(configuration)
            .AddAmqpBroker();
    }

    public static async Task<IApplicationBuilder> RunAmqpServices(
        this IApplicationBuilder app
    )
    {
        IAmqpBroker broker = app.ApplicationServices.GetRequiredService<IAmqpBroker>()!;
        AmqpBrokerConfigurator configurator = app.ApplicationServices.GetRequiredService<AmqpBrokerConfigurator>()!;
        await broker.Connect();
        await configurator.RegisterAmqpTopicConsumersAsync(broker);
        configurator.RegisterAmqpTopicPublishers(broker)
            .ToList()
            .ForEach(DomainEventPublisher.Instance.AddDomainEventSubscriber);

        return app;
    }

    private static IServiceCollection AddAmqpBrokerConfigurator(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ILogger<AmqpBrokerConfigurator> logger =
            services.BuildServiceProvider().GetRequiredService<ILogger<AmqpBrokerConfigurator>>()!;

        return services.AddSingleton(serviceProvider =>
        {
            return AmqpBrokerConfigurator.Create(
                logger,
                configuration.GetValue<string>("MessageBroker:AsyncApiPath")!,
                configuration.GetValue<string>("MessageBroker:Hostname")!,
                configuration.GetValue<string>("MessageBroker:Port")!,
                configuration.GetValue<string>("MessageBroker:VirtualHost")!,
                configuration.GetValue<string>("MessageBroker:Username")!,
                configuration.GetValue<string>("MessageBroker:Password")!
            );
        });
    }

    private static IServiceCollection AddAmqpBroker(
        this IServiceCollection services
    )
    {
        return services.AddSingleton(serviceProvider =>
        {
            AmqpBrokerConfigurator configurator = serviceProvider.GetRequiredService<AmqpBrokerConfigurator>()!;
            ILogger<DefaultAmqpBroker> logger = serviceProvider.GetRequiredService<ILogger<DefaultAmqpBroker>>()!;
            ILogger<MessageProcessorWithDI> mpLogger = serviceProvider.GetRequiredService<ILogger<MessageProcessorWithDI>>()!;

            return configurator
                .CreateMessageBroker(logger)
                .AddMessageProcessor(new MessageProcessorWithDI(serviceProvider, mpLogger));
        });
    }
}