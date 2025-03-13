using Domaincrafters.Application;
using Microsoft.Extensions.Logging;
using Simplifyme.Taskly.Domain.User;

namespace Simplifyme.Taskly.Application.Users;

public sealed record CreateUserInput
(
    string UserId, 
    string UserName
);

public sealed class CreateUser(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateUser> logger
) : IUseCase<CreateUserInput, Task<string>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateUser> _logger = logger;

    public async Task<string> Execute(CreateUserInput input)
    {
        UserId userId = new(input.UserId.ToString());

        User user = User.Create(input.UserName, userId);

        await _userRepository.Save(user);

        await _unitOfWork.Do();

        _logger.LogInformation("User {UserId} created", userId);

        return userId.Value;
    }
}