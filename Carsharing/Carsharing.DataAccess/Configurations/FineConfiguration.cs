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
            .HasColumnName("fine_id");

        builder.Property(f => f.TripId)
            .HasColumnName("fine_trip_id")
            .IsRequired();

        builder.Property(f => f.StatusId)
            .HasColumnName("fine_status_id")
            .IsRequired();

        builder.Property(f => f.Type)
            .HasColumnName("fine_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.Amount)
            .HasColumnName("fine_amount")
            .IsRequired();

        builder.Property(f => f.Date)
            .HasColumnName("fine_date")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_DATE");

        builder.HasOne(f => f.Trip)
            .WithMany(tr => tr.Fine)
            .HasForeignKey(f => f.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.FineStatus)
            .WithMany(s => s.Fines)
            .HasForeignKey(f => f.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_fine_amount_nonneg",
            "fine_amount >= 0"));
    }
}
