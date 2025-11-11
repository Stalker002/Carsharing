using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class FineConfiguration : IEntityTypeConfiguration<FineEntity>
{
    public void Configure(EntityTypeBuilder<FineEntity> builder)
    {
        builder.ToTable("fines");

        builder.HasKey(x => x.Id);

        builder.Property(f => f.Id)
            .HasColumnName("fine_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(f => f.TripId)
            .HasColumnName("fine_trip_id")
            .IsRequired();

        builder.Property(f => f.StatusId)
            .HasColumnName("fine_status_id")
            .IsRequired();

        builder.Property(f => f.Amount)
            .HasColumnName("fine_amount")
            .IsRequired();

        builder.Property(f => f.Date)
            .HasColumnName("fine_date")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_DATE");
    }
}