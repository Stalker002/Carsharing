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
            .IsRequired();

        builder.Property(tr => tr.EndTime)
            .HasColumnName("trip_end_time")
            .IsRequired(false);

        builder.Property(tr => tr.Duration)
            .HasColumnName("trip_duration")
            .IsRequired(false);

        builder.Property(tr => tr.Distance)
            .HasColumnName("trip_distance_km")
            .IsRequired(false);
    }
}