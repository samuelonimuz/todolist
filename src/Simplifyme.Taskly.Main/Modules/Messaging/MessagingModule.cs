using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Extensions;

namespace Simplifyme.Taskly.Main.Modules.Messaging;

public static class MessagingModule
{
    public static IServiceCollection AddMessagingModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return
            services
                .AddAmqpServices(configuration)
                .AddScoped<IControllerFactory, MessagingControllerFactory>();
    }


    public static async Task<IApplicationBuilder> RunMessagingModule(
        this WebApplication app
    )
    {
        return await app.RunAmqpServices();
    }
}