using Carsharing.Application.Abstractions;
using Carsharing.Application.Extensions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Carsharing.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<CarsharingDbContext>)
                    || d.ServiceType == typeof(CarsharingDbContext))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<CarsharingDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
        
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
