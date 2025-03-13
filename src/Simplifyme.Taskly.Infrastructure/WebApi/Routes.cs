using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simplifyme.Taskly.Infrastructure.WebApi.Controllers;
using Simplifyme.Taskly.Infrastructure.WebApi.Shared.Validators;

namespace Simplifyme.Taskly.Infrastructure.WebApi;

public static class Routes
{
    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapTodoListRoutes(app);
        MapUserRoutes(app);

        return app;
    }

    private static void MapTodoListRoutes(WebApplication app)
    {
        var todoListGroup = app.MapGroup("/api/todolists")
            .WithTags("TodoLists")
            .WithDescription("Endpoints related to todo list management")
            .WithOpenApi();

    }

    private static void MapUserRoutes(WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithDescription("Endpoints related to user management")
            .WithOpenApi();

        userGroup.MapPost("/", CreateUserController.Invoke)
            .WithName("CreateUser")
            .WithDescription("Creates a new user")
            .WithMetadata(new ConsumesAttribute(MediaTypeNames.Application.Json))
            .AddEndpointFilter<BodyValidatorFilter<CreateUserBody>>()
            .WithOpenApi();

        userGroup.MapGet("/by-name/{userName}", AllUsersByNameController.Invoke)
            .WithName("AllUsersByName")
            .WithDescription("Gets all users by name")
            .WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json))
            .WithOpenApi();
    }
}
