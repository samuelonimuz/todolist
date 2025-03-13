using Domaincrafters.Application;
using Simplifyme.Taskly.Application.Users;
using Simplifyme.Taskly.Infrastructure.Messaging.Controllers;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;

namespace Simplifyme.Taskly.Main.Modules.Messaging;

public class MessagingControllerFactory(
    IServiceProvider serviceProvider
) : IControllerFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IController<ConsumerContext> CreateController(ConsumerContext consumerContext)
    {
        return consumerContext.OperationId switch
        {
            "CreateUserWhenUserSubscribed" => BuildCreateUserController(),
            _ => throw new InvalidOperationException($"Operation with id {consumerContext.OperationId} not found")
        };
    }

    private CreateUserController BuildCreateUserController()
    {
        ILogger<CreateUserController> logger = _serviceProvider.GetRequiredService<ILogger<CreateUserController>>()!;
        IUseCase<CreateUserInput, Task<string>> createUser
            = _serviceProvider.GetRequiredService<IUseCase<CreateUserInput, Task<string>>>()!;
        return new CreateUserController(createUser, logger);
    }
}