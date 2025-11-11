using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class TripDetailConfiguration : IEntityTypeConfiguration<TripDetailEntity>
{
    public void Configure(EntityTypeBuilder<TripDetailEntity> builder)
    {
        builder.ToTable("trip_details");

        builder.HasKey(x => x.Id);

        builder.Property(d => d.Id)
            .HasColumnName("trip_detail_id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(d => d.TripId)
            .HasColumnName("trip_detail_trip_id")
            .IsRequired();

        builder.Property(d => d.StartLocation)
            .HasColumnName("trip_detail_start_location")
            .IsRequired();

        builder.Property(d => d.EndLocation)
            .HasColumnName("trip_detail_end_location")
            .IsRequired();

        builder.Property(d => d.InsuranceActive)
            .HasColumnName("trip_detail_insurance_active")
            .IsRequired(false);

        builder.Property(d => d.FuelUsed)
            .HasColumnName("trip_detail_fuel_used")
            .IsRequired(false);

        builder.Property(d => d.Refueled)
            .HasColumnName("trip_detail_refueled")
            .IsRequired(false);
    }
}