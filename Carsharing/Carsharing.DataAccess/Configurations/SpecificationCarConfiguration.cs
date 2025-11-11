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
            .HasColumnName("specification_car_id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(sp => sp.FuelType)
            .HasColumnName("specification_car_fuel_type")
            .IsRequired();

        builder.Property(sp => sp.Brand)
            .HasColumnName("specification_car_brand")
            .IsRequired();

        builder.Property(sp => sp.Model)
            .HasColumnName("specification_car_model")
            .IsRequired();

        builder.Property(sp => sp.Transmission)
            .HasColumnName("specification_car_transmission")
            .IsRequired();

        builder.Property(sp => sp.Year)
            .HasColumnName("specification_car_year")
            .IsRequired();

        builder.Property(sp => sp.VinNumber)
            .HasColumnName("specification_car_vin_number")
            .IsRequired();

        builder.Property(sp => sp.StateNumber)
            .HasColumnName("specification_car_state_number")
            .IsRequired();

        builder.Property(sp => sp.Mileage)
            .HasColumnName("specification_car_mileage")
            .IsRequired();

        builder.Property(sp => sp.MaxFuel)
            .HasColumnName("specification_car_max_fuel")
            .IsRequired();

        builder.Property(sp => sp.FuelPerKm)
            .HasColumnName("specification_car_fuel_per_km")
            .IsRequired();
    }
}