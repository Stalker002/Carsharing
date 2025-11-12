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
            .UseIdentityAlwaysColumn()
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
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Status)
            .WithMany(st => st.Bookings)
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Trip)
            .WithOne(tr => tr.Booking);

        //Чек на время что не превышает конец
    }
}