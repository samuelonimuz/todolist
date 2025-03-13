using Domaincrafters.Application;
using Simplifyme.Taskly.Application.Contracts.Data;
using Simplifyme.Taskly.Application.Contracts.Data.Filters;
using Simplifyme.Taskly.Application.Contracts.Ports;

namespace Simplifyme.Taskly.Application.Users;

public record AllUsersByNameInput(
    string Name
);

public class AllUsersByName(
    IAllUsersQuery allUsersQuery
) : IUseCase<AllUsersByNameInput, Task<IReadOnlyList<UserData>>>
{
    private readonly IAllUsersQuery _allUsersQuery = allUsersQuery;

    public async Task<IReadOnlyList<UserData>> Execute(AllUsersByNameInput input)
    {
        return await _allUsersQuery.Fetch(UserDataExpressions.UsernameContains(input.Name));
    }
}