using Domaincrafters.Domain;

namespace Simplifyme.Taskly.Domain.User;

public interface IUserRepository : IRepository<User, UserId>
{
}