using System.Linq.Expressions;

namespace Simplifyme.Taskly.Application.Contracts.Data.Filters;

public static class UserDataExpressions
{
    public static Expression<Func<UserData, bool>> UsernameContains(string username)
    {
        return userData => userData.UserName.ToLower().Contains(username.ToLower());
    }
}