namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared;

using System;
using System.Threading.Tasks;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class MessageProcessorWithDI(
    IServiceProvider serviceProvider,
    ILogger<MessageProcessorWithDI> logger
) : IAmqpMessageProcessor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<MessageProcessorWithDI> _logger = logger;

    public async Task ProcessMessage(ConsumerContext ctx)
    {
        _logger.LogInformation(
            "Processing message from exchange {ExchangeName} with event {EventName} and message: {Message}",
             ctx.ExchangeName, ctx.EventName, ctx.Message);

        IServiceProvider scopedProvider = _serviceProvider.CreateScope().ServiceProvider;

        await InvokeController(ctx, scopedProvider);
    }

    private async Task InvokeController(
        ConsumerContext ctx,
        IServiceProvider scopedProvider)
    {
        IControllerFactory controllerFactory = scopedProvider.GetService<IControllerFactory>()!;
        await controllerFactory.CreateController(ctx).Handle(ctx);
    }

}