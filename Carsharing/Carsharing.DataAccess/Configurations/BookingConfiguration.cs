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
            .HasColumnName("booking_id")
            .IsRequired()
            .ValueGeneratedOnAdd();

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
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.EndTime)
            .HasColumnName("booking_end_time")
            .IsRequired(false);
    }
}