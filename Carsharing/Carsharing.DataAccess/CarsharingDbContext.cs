using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess;

public class CarsharingDbContext : DbContext
{
    public CarsharingDbContext(DbContextOptions<CarsharingDbContext> options)
    : base(options)
    {
        
    }

    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}