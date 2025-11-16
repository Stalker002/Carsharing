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

        builder.Property(tr => tr.Id)
            .HasColumnName("trip_id")
            .UseIdentityAlwaysColumn()
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(tr => tr.BookingId)
            .HasColumnName("trip_booking_id")
            .IsRequired();

        builder.Property(tr => tr.StatusId)
            .HasColumnName("trip_status_id")
            .IsRequired();

        builder.Property(tr => tr.TariffType)
            .HasColumnName("trip_tariff_type")
            .IsRequired();

        builder.Property(tr => tr.StartTime)
            .HasColumnName("trip_start_time")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(tr => tr.EndTime)
            .HasColumnName("trip_end_time")
            .IsRequired(false);

        builder.Property(tr => tr.Duration)
            .HasColumnName("trip_duration")
            .HasDefaultValue(0m)
            .IsRequired(false);

        builder.Property(tr => tr.Distance)
            .HasColumnName("trip_distance_km")
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

        builder.HasOne(tr => tr.Status)
            .WithMany(s => s.Trip)
            .HasForeignKey(tr => tr.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чек на не негативные продолжительность и протяженность
        //Чек на не startTime > endTime
    }
}