using Domaincrafters.Domain;

namespace Simplifyme.Taskly.Domain.User;

public sealed class UserId(string? id = "") : UuidEntityId(id);

public class User : Entity<UserId>
{
    public string UserName { get; private set; }
    public bool IsSuspened { get; private set; }

    public static User Create(string userName, UserId? userId = null)
    {
        userId ??= new UserId();

        User user = new(userId, userName);

        user.ValidateState();

        return user;
    }

    private User(
        UserId id,
        string userName
    ) : base(id)
    {
        UserName = userName;
    }

    public void Suspend()
    {
        IsSuspened = true;
    }

    public override void ValidateState()
    {
        EnsureValidUserName(UserName);
    }

    private static void EnsureValidUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("User name cannot be empty");
    }
}