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

        builder.Property(b => b.StatusId)
            .IsRequired();

        builder.Property(b => b.CarId)
            .IsRequired();

        builder.Property(b => b.ClientId)
            .IsRequired();

        builder.Property(b => b.StartTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.EndTime)
            .IsRequired(false);

        builder.HasOne(b => b.Client)
            .WithMany(cl => cl.Bookings)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Car)
            .WithMany(c => c.Booking)
            .HasForeignKey(b => b.CarId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.BookingStatus)
            .WithMany(st => st.Bookings)
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Trip)
            .WithOne(tr => tr.Booking);

        //Чек на время что не превышает конец
    }
}