using System.Linq.Expressions;
using Simplifyme.Taskly.Application.Contracts.Data;

namespace Simplifyme.Taskly.Application.Contracts.Ports;

public interface IAllUsersQuery
{
    Task<IReadOnlyList<UserData>> Fetch(Expression<Func<UserData, bool>> filter);
}

public interface IUserByIdQuery
{
    Task<UserData?> Fetch(string id);
}

