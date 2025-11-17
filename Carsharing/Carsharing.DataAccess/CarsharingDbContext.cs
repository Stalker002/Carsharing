using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carsharing.DataAccess;

public class CarsharingDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public CarsharingDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<BillEntity> Bill { get; set; }
    public DbSet<BookingEntity> Booking { get; set; }
    public DbSet<CarEntity> Car { get; set; }
    public DbSet<CategoryEntity> Category { get; set; }
    public DbSet<ClientEntity> Client { get; set; }
    public DbSet<ClientDocumentEntity> ClientDocument { get; set; }
    public DbSet<FineEntity> Fine { get; set; }
    public DbSet<InsuranceEntity> Insurance { get; set; }
    public DbSet<MaintenanceEntity> Maintenance { get; set; }
    public DbSet<PaymentEntity> Payment { get; set; }
    public DbSet<PromocodeEntity> Promocode { get; set; }
    public DbSet<ReviewEntity> Review { get; set; }
    public DbSet<RoleEntity> Role { get; set; }
    public DbSet<SpecificationCarEntity> SpecificationCar { get; set; }
    public DbSet<StatusEntity> Status { get; set; }
    public DbSet<TariffEntity> Tariff { get; set; }
    public DbSet<TripDetailEntity> TripDetail { get; set; }
    public DbSet<TripEntity> Trip { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_configuration.GetConnectionString(nameof(CarsharingDbContext)))
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => { builder.AddConsole(); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarsharingDbContext).Assembly);
    }
}