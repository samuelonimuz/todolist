using System.Text.Json;
using Domaincrafters.Application;
using Microsoft.Extensions.Logging;
using Simplifyme.Taskly.Application.Users;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;
using Simplifyme.Taskly.Infrastructure.Messaging.Shared.Messages;

namespace Simplifyme.Taskly.Infrastructure.Messaging.Controllers;

public sealed class CreateUserController(
    IUseCase<CreateUserInput, Task<string>> createUser,
    ILogger<CreateUserController> logger
) : IController<ConsumerContext>
{
    private readonly IUseCase<CreateUserInput, Task<string>> _createUser = createUser;
    private readonly ILogger<CreateUserController> _logger = logger;

    public async Task Handle(ConsumerContext context)
    {
        JsonElement json = AmqpMessageConverter.ParseJson(context.Message);

        string userId = json.GetProperty("userId").GetString()
            ?? throw new ArgumentException("userId is required");
        string userName = json.GetProperty("userName").GetString()
            ?? throw new ArgumentException("userName is required");

        string createdUserId = await _createUser.Execute(new CreateUserInput(userId, userName));

        _logger.LogInformation("User {UserId} created", createdUserId);
    }
}
