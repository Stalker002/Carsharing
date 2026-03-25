using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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

        return new CarsharingDbContext(configuration);
    }
}
