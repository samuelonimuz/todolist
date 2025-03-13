using Simplifyme.Taskly.Domain.User;

namespace UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange + Act
        UserId userId = new();
        User user = User.Create("John Doe", userId);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("John Doe", user.UserName);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public void SuspendingUser_ShouldSetIsSuspendedToTrue()
    {
        // Arrange
        User user = User.Create("John Doe", new UserId());

        // Act
        user.Suspend();

        // Assert
        Assert.True(user.IsSuspened);
    }
}