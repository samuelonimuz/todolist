using Domaincrafters.Application;
using Simplifyme.Taskly.Application.Contracts.Data;
using Simplifyme.Taskly.Application.Contracts.Ports;
using Simplifyme.Taskly.Application.Users;
using Simplifyme.Taskly.Domain.User;

namespace Simplifyme.Taskly.Main.Modules.WebApi;

public static class UseCaseServices
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddCreateUser()
            .AddAllUsersByName();
    }

    private static IServiceCollection AddCreateUser(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<CreateUserInput, Task<string>>>(ServiceProvider =>
            {
                var userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
                var unitOfWork = ServiceProvider.GetRequiredService<IUnitOfWork>();
                var logger = ServiceProvider.GetRequiredService<ILogger<CreateUser>>();

                return new CreateUser(userRepository, unitOfWork, logger);
            });
    }

    private static IServiceCollection AddAllUsersByName(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<AllUsersByNameInput, Task<IReadOnlyList<UserData>>>>(ServiceProvider =>
            {
                var allUsersQuery = ServiceProvider.GetRequiredService<IAllUsersQuery>();
                return new AllUsersByName(allUsersQuery);
            });
    }
}