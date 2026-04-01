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
            .Setup(x => x.GetByLogin(login))
            .ReturnsAsync((User?)null);

        var (token, error) = await _usersService.Login(login, password);

        Assert.Null(token);
        Assert.Equal("Пользователь с таким логином не найден", error);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsError()
    {
        const string login = "testuser";
        const string password = "wrong_password";

        var testUser = User.Create(1, 1, login, "hashed_password").user;

        _userRepositoryMock
            .Setup(x => x.GetByLogin(login))
            .ReturnsAsync(testUser);

        _passwordHasherMock
            .Setup(x => x.Verify(password, testUser.Password))
            .Returns(false);

        var (token, error) = await _usersService.Login(login, password);

        Assert.Null(token);
        Assert.Equal("Неверный пароль", error);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        const string login = "testuser";
        const string password = "correct_password";
        const string expectedToken = "jwt_token_string";

        var testUser = User.Create(1, 1, login, "hashed_password").user;

        _userRepositoryMock
            .Setup(x => x.GetByLogin(login))
            .ReturnsAsync(testUser);

        _passwordHasherMock
            .Setup(x => x.Verify(password, testUser.Password))
            .Returns(true);

        _jwtProviderMock
            .Setup(x => x.GenerateToken(testUser))
            .Returns(expectedToken);

        var (token, error) = await _usersService.Login(login, password);

        Assert.Null(error);
        Assert.Equal(expectedToken, token);

        _userRepositoryMock.Verify(x => x.GetByLogin(login), Times.Once);
        _jwtProviderMock.Verify(x => x.GenerateToken(testUser), Times.Once);
    }
}