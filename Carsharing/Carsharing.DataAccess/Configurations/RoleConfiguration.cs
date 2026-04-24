using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(x => x.Id);

        builder.Property(r => r.Id)
            .HasColumnName("role_id");

        builder.Property(r => r.Name)
            .HasColumnName("role_name")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role);

        builder.HasData(
            new RoleEntity { Id = 1, Name = "Администратор" },
            new RoleEntity { Id = 2, Name = "Клиент" });
    }
}