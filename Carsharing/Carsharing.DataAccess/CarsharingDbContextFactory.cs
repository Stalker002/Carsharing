using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carsharing.DataAccess;

public class CarsharingDbContextFactory : IDesignTimeDbContextFactory<CarsharingDbContext>
{
    public CarsharingDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var startupProjectPath = Path.GetFullPath(Path.Combine(basePath, "..", "Carsharing"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(startupProjectPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CarsharingDbContext>();
        optionsBuilder
            .UseNpgsql(
                configuration.GetConnectionString(nameof(CarsharingDbContext)),
                options => options.UseNetTopologySuite())
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .EnableSensitiveDataLogging(false);

        return new CarsharingDbContext(optionsBuilder.Options);
    }
}
