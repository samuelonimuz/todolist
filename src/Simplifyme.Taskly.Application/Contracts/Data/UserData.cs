namespace Simplifyme.Taskly.Application.Contracts.Data;

public sealed record UserData
{
    public required string Id { get; init; }
    public required string UserName { get; init; }
}