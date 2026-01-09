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

        builder.Property(pr => pr.StatusId)
            .IsRequired();

        builder.Property(pr => pr.Code)
            .HasMaxLength(Promocode.MaxCodeLength)
            .IsRequired();

        builder.Property(pr => pr.Discount)
            .IsRequired();

        builder.Property(pr => pr.StartDate)
            .IsRequired();

        builder.Property(pr => pr.EndDate)
            .IsRequired();

        builder.HasIndex(pr => pr.Code)
            .IsUnique();

        builder.HasMany(pr => pr.Bill)
            .WithOne(b => b.Promocode);

        builder.HasOne(pr => pr.PromocodeStatus)
            .WithMany(s => s.Promocodes)
            .HasForeignKey(pr => pr.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чек на не startDate > endDate
    }
}