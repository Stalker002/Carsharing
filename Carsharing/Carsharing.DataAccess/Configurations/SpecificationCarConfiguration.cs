using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class SpecificationCarConfiguration : IEntityTypeConfiguration<SpecificationCarEntity>
{
    public void Configure(EntityTypeBuilder<SpecificationCarEntity> builder)
    {
        builder.ToTable("specifications_car");

        builder.HasKey(x => x.Id);

        builder.Property(sp => sp.FuelType)
            .IsRequired();
        //enum

        builder.Property(sp => sp.Brand)
            .HasMaxLength(SpecificationCar.MaxBrandLength)
            .IsRequired();

        builder.Property(sp => sp.Model)
            .HasMaxLength(SpecificationCar.MaxModelLength)
            .IsRequired();

        builder.Property(sp => sp.Transmission)
            .IsRequired();
        //enum

        builder.Property(sp => sp.Year)
            .IsRequired();

        builder.Property(sp => sp.VinNumber)
            .HasMaxLength(SpecificationCar.MaxVinNumberLength)
            .IsRequired();

        builder.Property(sp => sp.StateNumber)
            .HasMaxLength(SpecificationCar.MaxStateNumberLength)
            .IsRequired();

        builder.Property(sp => sp.Mileage)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(sp => sp.MaxFuel)
            .IsRequired();

        builder.Property(sp => sp.FuelPerKm)
            .HasDefaultValue(0.08)
            .IsRequired();

        builder.HasIndex(sp => sp.StateNumber)
            .IsUnique();

        builder.HasIndex(sp => sp.VinNumber)
            .IsUnique();
        //Чек на не негативные значения километража и топлива
        //Чек на то что машина не ниже 1900 и не в будущем

        builder.HasOne(sp => sp.Car)
            .WithOne(c => c.SpecificationCar);
    }
}