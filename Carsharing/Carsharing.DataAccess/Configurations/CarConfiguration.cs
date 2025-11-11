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
            .ValueGeneratedOnAdd();
    }
}