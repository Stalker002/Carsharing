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

        builder.Property(sp => sp.Id)
            .HasColumnName("specification_car_id");

        builder.Property(sp => sp.FuelType)
            .HasColumnName("specification_car_fuel_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sp => sp.Brand)
            .HasColumnName("specification_car_brand")
            .HasMaxLength(SpecificationCar.MaxBrandLength)
            .IsRequired();

        builder.Property(sp => sp.Model)
            .HasColumnName("specification_car_model")
            .HasMaxLength(SpecificationCar.MaxModelLength)
            .IsRequired();

        builder.Property(sp => sp.Transmission)
            .HasColumnName("specification_car_transmission")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sp => sp.Year)
            .HasColumnName("specification_car_year")
            .IsRequired();

        builder.Property(sp => sp.VinNumber)
            .HasColumnName("specification_car_vin_number")
            .HasMaxLength(SpecificationCar.MaxVinNumberLength)
            .IsRequired();

        builder.Property(sp => sp.StateNumber)
            .HasColumnName("specification_car_state_number")
            .HasMaxLength(SpecificationCar.MaxStateNumberLength)
            .IsRequired();

        builder.Property(sp => sp.Mileage)
            .HasColumnName("specification_car_mileage")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(sp => sp.MaxFuel)
            .HasColumnName("specification_car_max_fuel")
            .IsRequired();

        builder.Property(sp => sp.FuelPerKm)
            .HasColumnName("specification_fuel_per_km")
            .HasDefaultValue(0.08)
            .IsRequired();

        builder.HasIndex(sp => sp.StateNumber)
            .IsUnique();

        builder.HasIndex(sp => sp.VinNumber)
            .IsUnique();

        builder.HasOne(sp => sp.Car)
            .WithOne(c => c.SpecificationCar);

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_specification_car_max_fuel_positive",
            "specification_car_max_fuel >= 0"));
        builder.ToTable(t => t.HasCheckConstraint(
            "chk_specification_car_mileage_non_negative",
            "specification_car_mileage >= 0"));
        builder.ToTable(t => t.HasCheckConstraint(
            "chk_specification_car_year_range",
            "specification_car_year >= 1900 AND specification_car_year <= EXTRACT(year FROM CURRENT_DATE) + 1"));
        builder.ToTable(t => t.HasCheckConstraint(
            "specifications_car_specification_fuel_per_km_check",
            "specification_fuel_per_km >= 0"));
    }
}
