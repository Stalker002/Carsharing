using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carsharing.DataAccess;

public class CarsharingDbContextFactory : IDesignTimeDbContextFactory<CarsharingDbContext>
{
    private const string ConnectionStringName = nameof(CarsharingDbContext);

    public CarsharingDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var startupProjectPath = Path.GetFullPath(Path.Combine(basePath, "..", "Carsharing"));
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(startupProjectPath)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CarsharingDbContext>();
        optionsBuilder
            .UseNpgsql(
                configuration.GetConnectionString(ConnectionStringName),
                options => options.UseNetTopologySuite())
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .EnableSensitiveDataLogging(false);

        return new CarsharingDbContext(optionsBuilder.Options);
    }
}