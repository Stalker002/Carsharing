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

        builder.Property(c => c.Id)
            .HasColumnName("car_id")
            .ValueGeneratedOnAdd()
            .UseIdentityAlwaysColumn()
            .IsRequired();

        builder.Property(c => c.StatusId)
            .HasColumnName("car_status_id")
            .IsRequired();

        builder.Property(c => c.TariffId)
            .HasColumnName("car_tariff_id")
            .IsRequired();

        builder.Property(c => c.CategoryId)
            .HasColumnName("car_category_id")
            .IsRequired();

        builder.Property(c => c.SpecificationId)
            .HasColumnName("car_specification_id")
            .IsRequired();

        builder.Property(c => c.Location)
            .HasColumnName("car_location")
            .IsRequired()
            .HasMaxLength(Car.MaxLocationLength);

        builder.Property(c => c.FuelLevel)
            .HasColumnName("car_fuel_level")
            .IsRequired()
            .HasDefaultValue(0)
            .ValueGeneratedOnAdd();

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

        builder.HasOne(c => c.Status)
            .WithMany(st => st.Cars)
            .HasForeignKey(c => c.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Сделать чек на положительное топливо
    }
}