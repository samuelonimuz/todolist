using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Application.Contracts.Data;
using Simplifyme.Taskly.Application.Contracts.Ports;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Queries;

public sealed class AllUsersQuery(
    QueryContextBase context
) : IAllUsersQuery
{
    private readonly QueryContextBase _context = context;

    public async Task<IReadOnlyList<UserData>> Fetch(Expression<Func<UserData, bool>> filter)
    {
        return await _context.Users
            .Where(filter)
            .ToListAsync();
    }
}
