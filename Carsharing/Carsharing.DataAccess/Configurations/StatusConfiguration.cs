using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> builder)
    {
        builder.ToTable("status");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Id)
            .HasColumnName("status_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(st => st.Name)
            .HasColumnName("status_name")
            .IsRequired();

        builder.Property(st => st.Description)
            .HasColumnName("status_description")
            .IsRequired();
    }
}