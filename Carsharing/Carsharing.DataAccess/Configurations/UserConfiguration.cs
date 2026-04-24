using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(u => u.Id)
            .HasColumnName("user_id");

        builder.Property(u => u.RoleId)
            .HasColumnName("user_role_id")
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(u => u.Login)
            .HasColumnName("user_login")
            .HasMaxLength(User.MaxLoginLength)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasColumnName("user_password_hash")
            .HasMaxLength(User.MaxPasswordLength)
            .IsRequired();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Client)
            .WithOne(cl => cl.User);

        builder.HasIndex(u => u.Login)
            .IsUnique();
    }
}