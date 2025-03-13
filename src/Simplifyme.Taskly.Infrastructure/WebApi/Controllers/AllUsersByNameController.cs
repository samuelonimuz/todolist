using Domaincrafters.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Simplifyme.Taskly.Application.Contracts.Data;
using Simplifyme.Taskly.Application.Users;
using Simplifyme.Taskly.Infrastructure.WebApi.Controllers.Responses;

namespace Simplifyme.Taskly.Infrastructure.WebApi.Controllers;

public sealed class AllUsersByNameController
{
    public static async Task<Results<Ok<Users>, BadRequest>> Invoke(
        [AsParameters] AllUsersByNameParameters parameters,
        [FromServices] IUseCase<AllUsersByNameInput, Task<IReadOnlyList<UserData>>> allUsersByName
    )
    {
        AllUsersByNameInput input = new (
            parameters.UserName
        );

        IReadOnlyList<UserData> users = await allUsersByName.Execute(input);

        return TypedResults.Ok(BuildResponse(users));

    }

    private static Users BuildResponse(IReadOnlyList<UserData> users)
    {
        return new Users(
            users.Select(user => new User(
                user.Id.ToString(),
                user.UserName
            ))
        );
    }
}

public sealed record AllUsersByNameParameters
{
    public required string UserName { get; init; }
}