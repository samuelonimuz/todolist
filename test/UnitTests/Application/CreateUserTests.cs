using Simplifyme.Taskly.Application.Users;
using Simplifyme.Taskly.Domain.User;
using UnitTests.Shared;

namespace UnitTests.Application;

public class CreateUserTests
{
    private readonly DefaultMockUserRepository _userRepository;
    private readonly DefaultMockUnitOfWork _unitOfWork;
    private readonly DefaultMockTestLogger<CreateUser> _logger;
    private readonly CreateUser _createUser;
    private readonly string defaultUserId = new UserId().Value;

    public CreateUserTests()
    {
        _userRepository = new DefaultMockUserRepository();
        _unitOfWork = new DefaultMockUnitOfWork();
        _logger = new DefaultMockTestLogger<CreateUser>();
        _createUser = new CreateUser(
            _userRepository,
            _unitOfWork,
            _logger
        );
    }

    [Fact]
    public async Task Execute_ValidInput_ShouldCreateUser()
    {
        // Arrange
        CreateUserInput input = new (
            defaultUserId,
            "TestUser"
        );

        // Act
        var result = await _createUser.Execute(input);

        // Assert
        Assert.Equal(input.UserId, result);
        Assert.Single(_userRepository.SavedUsers);

        var savedUser = _userRepository.SavedUsers[0];
        Assert.Equal(input.UserId, savedUser.Id.Value);
        Assert.Equal(input.UserName, savedUser.UserName);
        Assert.Contains($"User {input.UserId} created", _logger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_EmptyUsername_ShouldThrowException()
    {
        // Arrange
        var input = new CreateUserInput(
            defaultUserId,
            ""
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await _createUser.Execute(input));
        Assert.Contains("User name cannot be empty", exception.Message);
        Assert.Empty(_userRepository.SavedUsers);
    }

    [Fact]
    public async Task Execute_WithRepositoryFailure_ShouldPropagateException()
    {
        // Arrange
        var input = new CreateUserInput(
            defaultUserId,
            "TestUser"
        );
        _userRepository.ThrowOnSave = new Exception("Database error");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await _createUser.Execute(input));
        Assert.Equal("Database error", exception.Message);
    }
}