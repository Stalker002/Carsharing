using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class PromocodeConfiguration : IEntityTypeConfiguration<PromocodeEntity>
{
    public void Configure(EntityTypeBuilder<PromocodeEntity> builder)
    {
        builder.ToTable("promocodes");

        builder.HasKey(x => x.Id);

        builder.Property(pr => pr.Id)
            .HasColumnName("promocode_id")
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(pr => pr.StatusId)
            .HasColumnName("promocode_status_id")
            .IsRequired();

        builder.Property(pr => pr.Code)
            .HasColumnName("promocode_code")
            .HasMaxLength(Promocode.MaxCodeLength)
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

        builder.HasIndex(pr => pr.Code)
            .IsUnique();

        builder.HasMany(pr => pr.Bill)
            .WithOne(b => b.Promocode);

        builder.HasOne(pr => pr.Status)
            .WithMany(s => s.Promocodes)
            .HasForeignKey(pr => pr.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чек на не startDate > endDate
    }
}