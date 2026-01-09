using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class TripConfiguration : IEntityTypeConfiguration<TripEntity>
{
    public void Configure(EntityTypeBuilder<TripEntity> builder)
    {
        builder.ToTable("trips");

        builder.HasKey(x => x.Id);

        builder.Property(tr => tr.BookingId)
            .IsRequired();

        builder.Property(tr => tr.StatusId)
            .IsRequired();

        builder.Property(tr => tr.TariffType)
            .IsRequired();

        builder.Property(tr => tr.StartTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(tr => tr.EndTime)
            .IsRequired(false);

        builder.Property(tr => tr.Duration)
            .HasDefaultValue(0m)
            .IsRequired(false);

        builder.Property(tr => tr.Distance)
            .HasDefaultValue(0m)
            .IsRequired(false);

        builder.HasIndex(tr => tr.BookingId)
            .IsUnique();

        builder.HasOne(tr => tr.TripDetail)
            .WithOne(d => d.Trip);

        builder.HasOne(tr => tr.Bill)
            .WithOne(b => b.Trip);

        builder.HasMany(tr => tr.Fine)
            .WithOne(f => f.Trip);

        builder.HasOne(tr => tr.Booking)
            .WithOne(b => b.Trip)
            .HasForeignKey<TripEntity>(tr => tr.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tr => tr.TripStatus)
            .WithMany(s => s.Trip)
            .HasForeignKey(tr => tr.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чек на не негативные продолжительность и протяженность
        //Чек на не startTime > endTime
    }
}