using Aornis;
using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Domain.User;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class UserRepository(
    DomainContextBase context
) : IUserRepository
{
    private readonly DomainContextBase _context = context;

    public Task<Optional<User>> ById(UserId id)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.Id == id)
            .ContinueWith(task => Optional.Of(task.Result));
    }

    public Task Remove(User entity)
    {
        _context.Users.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task Save(User entity)
    {
        bool exists = await _context.Users
            .AnyAsync(u => u.Id == entity.Id);

        if (exists)
        {
            _context.Users.Update(entity);
            return;
        }
        
        _context.Users.Add(entity);
    }
}