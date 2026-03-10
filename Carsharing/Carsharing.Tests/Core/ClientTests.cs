using Carsharing.Core.Models;

namespace Carsharing.Tests.Core;

public class ClientTests
{
    [Fact]
    public void Create_ValidData_ReturnsClient()
    {
        var (client, error) = Client.Create(1, 100, "Ivan", "Ivanov", "+375291234567", "test@test.com");

        Assert.NotNull(client);
        Assert.Equal(string.Empty, error);
        Assert.Equal("Ivan", client.Name);
        Assert.Equal("test@test.com", client.Email);
    }

    [Theory][InlineData("invalid-email")]
    [InlineData("test@.com")]
    [InlineData("test.com")][InlineData("")]
    public void Create_InvalidEmail_ReturnsError(string invalidEmail)
    {
        var (client, error) = Client.Create(1, 100, "Ivan", "Ivanov", "+375291234567", invalidEmail);

        Assert.Null(client);
        Assert.NotEmpty(error);
    }

    [Theory][InlineData("+375991234567")]
    [InlineData("8029123456")]   
    [InlineData("123456789")]   
    public void Create_InvalidPhone_ReturnsError(string invalidPhone)
    {
        var (client, error) = Client.Create(1, 100, "Ivan", "Ivanov", invalidPhone, "test@test.com");

        Assert.Null(client);
        Assert.Contains("Phone number should be in format", error);
    }
}