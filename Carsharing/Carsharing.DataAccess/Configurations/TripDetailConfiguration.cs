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

        builder.Property(d => d.TripId)
            .IsRequired();

        builder.Property(d => d.StartLocation)
            .IsRequired();

        builder.Property(d => d.EndLocation)
            .IsRequired();

        builder.Property(d => d.InsuranceActive)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(d => d.FuelUsed)
            .HasDefaultValue(0m)
            .IsRequired(false);

        builder.Property(d => d.Refueled)
            .HasDefaultValue(0m)
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