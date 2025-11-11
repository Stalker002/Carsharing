using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<BillEntity>
{
   public void Configure(EntityTypeBuilder<BillEntity> builder)
   {
       builder.ToTable("bills");

        builder.HasKey(x => x.Id);

        builder.Property(b => b.Id)
            .HasColumnName("bill_id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(b => b.TripId)
            .HasColumnName("bill_trip_id")
            .IsRequired();

        builder.Property(b => b.PromocodeId)
            .HasColumnName("bill_promocode_id")
            .IsRequired(false);

        builder.Property(b => b.StatusId)
            .HasColumnName("bill_status_id")
            .IsRequired();

        builder.Property(b => b.IssueDate)
            .HasColumnName("bill_issue_date")
            .IsRequired();

        builder.Property(b => b.Amount)
            .HasColumnName("bill_amount")
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.RemainingAmount)
            .HasColumnName("bill_remaining_amount")
            .IsRequired(false)
            .ValueGeneratedOnAdd();
    }
}