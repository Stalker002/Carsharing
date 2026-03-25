using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(x => x.Id);

        builder.Property(b => b.Id)
            .HasColumnName("booking_id");

        builder.Property(b => b.StatusId)
            .HasColumnName("booking_status_id")
            .IsRequired();

        builder.Property(b => b.CarId)
            .HasColumnName("booking_car_id")
            .IsRequired();

        builder.Property(b => b.ClientId)
            .HasColumnName("booking_client_id")
            .IsRequired();

        builder.Property(b => b.StartTime)
            .HasColumnName("booking_start_time")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.EndTime)
            .HasColumnName("booking_end_time")
            .IsRequired(false);

        builder.HasOne(b => b.Client)
            .WithMany(cl => cl.Bookings)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Car)
            .WithMany(c => c.Booking)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.BookingStatus)
            .WithMany(st => st.Bookings)
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Trip)
            .WithOne(tr => tr.Booking);

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_booking_times",
            "booking_start_time IS NULL OR booking_end_time IS NULL OR booking_start_time < booking_end_time"));
    }
}
