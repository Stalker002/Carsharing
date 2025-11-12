using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles");

        builder.Property(r => r.Id)
            .HasColumnName("role_id")
            .UseIdentityAlwaysColumn()
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Name)
            .HasColumnName("role_name")
            .IsRequired();

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role);
    }
}