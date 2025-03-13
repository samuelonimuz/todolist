using Aornis;
using Simplifyme.Taskly.Domain.User;

namespace UnitTests.Shared;

public class DefaultMockUserRepository : IUserRepository
{
    public List<User> SavedUsers { get; } = new List<User>();
    public List<User> Users { get; } = new List<User>();
    public Exception? ThrowOnSave { get; set; }

    public Task Save(User user)
    {
        if (ThrowOnSave != null)
            throw ThrowOnSave;

        SavedUsers.Add(user);

        var existingIndex = Users.FindIndex(u => u.Id.Equals(user.Id));
        if (existingIndex >= 0)
            Users[existingIndex] = user;
        else
            Users.Add(user);

        return Task.CompletedTask;
    }

    public Task<Optional<User>> ById(UserId userId)
    {
        var user = Users.FirstOrDefault(u => u.Id.Value == userId.Value);
        return Task.FromResult(Optional.Of<User>(user));
    }

    public Task<User?> GetById(UserId userId)
    {
        return Task.FromResult(Users.FirstOrDefault(u => u.Id.Value == userId.Value));
    }

    public Task Remove(User entity)
    {
        throw new NotImplementedException();
    }
}