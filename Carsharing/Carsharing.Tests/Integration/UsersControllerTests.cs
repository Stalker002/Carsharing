using System.Net;
using System.Net.Http.Json;
using Carsharing.Contracts;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Tests.Integration;

public class UsersControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkAndToken()
    {
        const string testLogin = "integration_user";
        const string testPassword = "StrongPassword123!";
        
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(testPassword);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();
            
            db.Users.Add(new UserEntity 
            { 
                RoleId = 1, 
                Login = testLogin, 
                Password = hashedPassword 
            });
            await db.SaveChangesAsync();
        }

        var request = new LoginRequest(testLogin, testPassword);

        var response = await _client.PostAsJsonAsync("/Users/login", request);
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorText = await response.Content.ReadAsStringAsync();
            throw new Exception($"Логин провалился. Статус: {response.StatusCode}. Ошибка: {errorText}");
        }

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("token", responseString);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsBadRequest()
    {
        var request = new LoginRequest("non_existent_user", "wrong_pass");

        var response = await _client.PostAsJsonAsync("/Users/login", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Неверный логин или пароль", responseString); 
    }
}