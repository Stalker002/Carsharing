using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class CarConfiguration : IEntityTypeConfiguration<CarEntity>
{
    public void Configure(EntityTypeBuilder<CarEntity> builder)
    {
        builder.ToTable("cars");

        builder.HasKey(x => x.Id);

        builder.Property(c => c.StatusId)
            .IsRequired();

        builder.Property(c => c.TariffId)
            .IsRequired();

        builder.Property(c => c.CategoryId)
            .IsRequired();

        builder.Property(c => c.SpecificationId)
            .IsRequired();

        builder.Property(c => c.Location)
            .IsRequired()
            .HasMaxLength(Car.MaxLocationLength);

        builder.Property(c => c.FuelLevel)
            .IsRequired()
            .HasDefaultValue(0)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ImagePath)
            .IsRequired(false);

        builder.HasIndex(c => c.SpecificationId)
            .IsUnique();

        builder.HasMany(c => c.Booking)
            .WithOne(b => b.Car);

        builder.HasMany(c => c.Reviews)
            .WithOne(r => r.Car);

        builder.HasMany(c => c.Maintenance)
            .WithOne(m => m.Car);

        builder.HasMany(c => c.Insurance)
            .WithOne(i => i.Car);

        builder.HasOne(c => c.Category)
            .WithMany(c => c.Cars)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Tariff)
            .WithMany(t => t.Cars)
            .HasForeignKey(c => c.TariffId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.SpecificationCar)
            .WithOne(sp => sp.Car)
            .HasForeignKey<CarEntity>(c => c.SpecificationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.CarStatus)
            .WithMany(st => st.Cars)
            .HasForeignKey(c => c.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Сделать чек на положительное топливо
    }
}