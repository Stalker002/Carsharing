using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class RoleConfiguration
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(r => r.Id)
            .IsRequired();

        builder.Property(r => r.Name)
            .IsRequired();
    }
}