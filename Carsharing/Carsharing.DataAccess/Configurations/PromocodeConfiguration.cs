using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class PromocodeConfiguration : IEntityTypeConfiguration<PromocodeEntity>
{
    public void Configure(EntityTypeBuilder<PromocodeEntity> builder)
    {
        builder.ToTable("promocode");

        builder.HasKey(x => x.Id);

        builder.Property(pr => pr.Id)
            .HasColumnName("promocode_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(pr => pr.StatusId)
            .HasColumnName("promocode_status_id")
            .IsRequired();

        builder.Property(pr => pr.Code)
            .HasColumnName("promocode_code")
            .IsRequired();

        builder.Property(pr => pr.Discount)
            .HasColumnName("promocode_discount")
            .IsRequired();

        builder.Property(pr => pr.StartDate)
            .HasColumnName("promocode_start_date")
            .IsRequired();

        builder.Property(pr => pr.EndDate)
            .HasColumnName("promocode_end_date")
            .IsRequired();
    }
}