using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class UserTests
{
    [Fact]
    public void Create_ValidData_ReturnsUserAndEmptyError()
    {
        const int id = 1;
        const int roleId = 2;
        const string login = "testuser";
        const string password = "StrongPassword123!";

        var (user, error) = User.Create(id, roleId, login, password);

        Assert.NotNull(user);
        Assert.Equal(string.Empty, error);
        Assert.Equal(login, user.Login);
    }

    [Fact]
    public void Create_EmptyPassword_ReturnsError()
    {
        const string emptyPassword = "";

        var (user, error) = User.Create(1, 2, "testuser", emptyPassword);

        Assert.Null(user);
        Assert.NotEmpty(error);
        Assert.Equal("Password can't be empty", error);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("abc")]
    public void Create_ShortPassword_ReturnsLengthError(string shortPassword)
    {
        var (user, error) = User.Create(1, 2, "testuser", shortPassword);

        Assert.Null(user);
        Assert.Contains("Password can't be shoter than 6 symbols", error);
    }
}