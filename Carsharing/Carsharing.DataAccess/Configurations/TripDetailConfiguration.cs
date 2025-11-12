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
            .UseIdentityAlwaysColumn()
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
            .HasDefaultValue(false)
            .IsRequired(false);

        builder.Property(d => d.FuelUsed)
            .HasColumnName("trip_detail_fuel_used")
            .HasDefaultValue(0)
            .IsRequired(false);

        builder.Property(d => d.Refueled)
            .HasColumnName("trip_detail_refueled")
            .HasDefaultValue(0)
            .IsRequired(false);

        builder.HasIndex(d => d.TripId)
            .IsUnique();

        builder.HasOne(d => d.Trip)
            .WithOne(tr => tr.TripDetail)
            .HasForeignKey<TripDetailEntity>(d => d.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        //Чеки, что все числа >0
    }
}