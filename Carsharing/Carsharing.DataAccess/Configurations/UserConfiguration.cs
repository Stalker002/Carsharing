using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users")
        
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Id)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(u => u.RoleId)
            .HasColumnName("user_role_id")
            .IsRequired();

        builder.Property(u => u.Login)
            .HasColumnName("user_login")
            .HasMaxLength(User.MaxLoginLength)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("user_password_hash")
            .HasMaxLength(User.MaxPasswordLength)
            .IsRequired();
    }
}