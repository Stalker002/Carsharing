using Carsharing.Application.Abstractions;
using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Moq;

namespace Carsharing.Tests.Application;

public class UsersServiceTests
{
    private readonly Mock<IUsersRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly UsersService _usersService;

    public UsersServiceTests()
    {
        _userRepositoryMock = new Mock<IUsersRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtProviderMock = new Mock<IJwtProvider>();

        _usersService = new UsersService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtProviderMock.Object);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsError()
    {
        const string login = "unknown_user";
        const string password = "password123";

        _userRepositoryMock
            .Setup(x => x.GetByLogin(login, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var (token, error) = await _usersService.Login(login, password, CancellationToken.None);

        Assert.Null(token);
        Assert.Equal("Пользователь с таким логином не найден", error);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsError()
    {
        const string login = "testuser";
        const string password = "wrong_password";

        var testUser = User.Restore(1, 1, login, "hashed_password");

        _userRepositoryMock
            .Setup(x => x.GetByLogin(login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testUser);

        _passwordHasherMock
            .Setup(x => x.Verify(password, testUser.Password))
            .Returns(false);

        var (token, error) = await _usersService.Login(login, password, CancellationToken.None);

        Assert.Null(token);
        Assert.Equal("Неверный пароль", error);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        const string login = "testuser";
        const string password = "correct_password";
        const string expectedToken = "jwt_token_string";

        var testUser = User.Restore(1, 1, login, "hashed_password");

        _userRepositoryMock
            .Setup(x => x.GetByLogin(login, It.IsAny<CancellationToken>()))
            .ReturnsAsync(testUser);

        _passwordHasherMock
            .Setup(x => x.Verify(password, testUser.Password))
            .Returns(true);

        _jwtProviderMock
            .Setup(x => x.GenerateToken(testUser))
            .Returns(expectedToken);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        var (token, error) = await _usersService.Login(login, password, specificToken);

        Assert.Null(error);
        Assert.Equal(expectedToken, token);

        _userRepositoryMock.Verify(x => x.GetByLogin(login, specificToken), Times.Once);
        _jwtProviderMock.Verify(x => x.GenerateToken(testUser), Times.Once);
    }

    [Fact]
    public async Task CreateUser_HashesPasswordInServiceBeforeSaving()
    {
        var plainUser = User.Create(0, 2, "new_user", "StrongPassword123!").user;

        _passwordHasherMock
            .Setup(x => x.Generate("StrongPassword123!"))
            .Returns("hashed_password");

        _userRepositoryMock
            .Setup(x => x.CreateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        var userId = await _usersService.CreateUser(plainUser, specificToken);

        Assert.Equal(10, userId);
        _passwordHasherMock.Verify(x => x.Generate("StrongPassword123!"), Times.Once);
        _userRepositoryMock.Verify(
            x => x.CreateUser(It.Is<User>(u =>
                u.RoleId == 2 &&
                u.Login == "new_user" &&
                u.Password == "hashed_password"), specificToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateUser_HashesOnlyNewPasswordAndPassesHashToRepository()
    {
        var existingUser = User.Restore(7, 1, "existing_user", "existing_hash");

        _userRepositoryMock
            .Setup(x => x.GetUserById(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingUser]);

        _passwordHasherMock
            .Setup(x => x.Generate("NewStrongPassword123!"))
            .Returns("new_hash");

        _userRepositoryMock
            .Setup(x => x.UpdateUser(7, 2, "updated_user", "new_hash", It.IsAny<CancellationToken>()))
            .ReturnsAsync(7);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        var userId = await _usersService.UpdateUser(7, 2, "updated_user", "NewStrongPassword123!", specificToken);

        Assert.Equal(7, userId);
        _passwordHasherMock.Verify(x => x.Generate("NewStrongPassword123!"), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateUser(7, 2, "updated_user", "new_hash", specificToken), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_WithoutPassword_DoesNotRehashExistingPassword()
    {
        var existingUser = User.Restore(8, 1, "existing_user", "existing_hash");

        _userRepositoryMock
            .Setup(x => x.GetUserById(8, It.IsAny<CancellationToken>()))
            .ReturnsAsync([existingUser]);

        _userRepositoryMock
            .Setup(x => x.UpdateUser(8, 1, "renamed_user", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(8);

        using var cts = new CancellationTokenSource();
        CancellationToken specificToken = cts.Token;

        var userId = await _usersService.UpdateUser(8, 1, "renamed_user", null, specificToken);

        Assert.Equal(8, userId);
        _passwordHasherMock.Verify(x => x.Generate(It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(x => x.UpdateUser(8, 1, "renamed_user", null, specificToken), Times.Once);
    }
}
