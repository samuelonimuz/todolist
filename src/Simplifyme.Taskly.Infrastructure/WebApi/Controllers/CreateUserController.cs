using System.ComponentModel.DataAnnotations;
using Domaincrafters.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Simplifyme.Taskly.Application.Users;

namespace Simplifyme.Taskly.Infrastructure.WebApi.Controllers;

public sealed class CreateUserController
{
    public static async Task<Results<Created, BadRequest>> Invoke(
        [FromBody] CreateUserBody body,
        [FromServices] IUseCase<CreateUserInput, Task<string>> createUser
    )
    {
        Console.WriteLine($"CreateUserController invoked with body: {body}");
        CreateUserInput input = new (
            body.UserId.ToString(), body.UserName
        );

        string userId = await createUser.Execute(input);

        return TypedResults.Created($"/users/{userId}");
    }
}

public sealed record CreateUserBody
{
    [Required]
    public required Guid UserId { get; init; }
    [Required]
    public required string UserName { get; init; }
}

