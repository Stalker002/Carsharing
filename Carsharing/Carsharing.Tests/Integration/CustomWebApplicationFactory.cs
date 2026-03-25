using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CarsharingDbContext>));
            if (descriptor != null) services.Remove(descriptor);
            
            services.AddDbContext<CarsharingDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
        
                options.ConfigureWarnings(warnings => 
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }
    
    public string GenerateTestToken(int userId, int roleId)
    {
        using var scope = Services.CreateScope();
        var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
        
        var testUser = User.Create(userId, roleId, "testuser", "password").user!;
        
        return jwtProvider.GenerateToken(testUser);
    }
}