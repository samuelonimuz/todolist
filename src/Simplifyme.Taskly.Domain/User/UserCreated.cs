using Simplifyme.Taskly.Domain.Shared;


namespace Simplifyme.Taskly.Domain.User;


public class UserCreated : TasklyDomainEvent
{
        public string UserId { get; }
        public string UserName { get; }

        public static UserCreated Create(UserId userId, string userName)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName cannot be empty.", nameof(userName));

            return new UserCreated(userId.Value, userName, DateTime.UtcNow);
        }

        private UserCreated(string userId, string userName, DateTime occurredOn)
            : base(nameof(UserCreated))
        {
            UserId = userId;
            UserName = userName;
        }
}