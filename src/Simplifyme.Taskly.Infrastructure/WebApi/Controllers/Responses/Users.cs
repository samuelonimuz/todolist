namespace Simplifyme.Taskly.Infrastructure.WebApi.Controllers.Responses;

public sealed record Users(
    IEnumerable<User> Data
);