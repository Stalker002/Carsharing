using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.RoleId)
            .IsRequired();

        builder.Property(u => u.Login)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .IsRequired();
    }
}