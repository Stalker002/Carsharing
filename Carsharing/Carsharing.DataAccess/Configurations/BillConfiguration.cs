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
            .UseIdentityAlwaysColumn()
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
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(b => b.IssueDate)
            .HasColumnName("bill_issue_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(b => b.Amount)
            .HasColumnName("bill_amount")
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.RemainingAmount)
            .HasColumnName("bill_remaining_amount")
            .IsRequired(false)
            .ValueGeneratedOnAdd();

        builder.HasIndex(b => b.TripId)
            .IsUnique();

        builder.HasMany(b => b.Payments)
                    .WithOne(p => p.Bill);

        builder.HasOne(b => b.Trip)
            .WithOne(tr => tr.Bill)
            .HasForeignKey<BillEntity>(b => b.TripId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(b => b.Status)
            .WithMany(s => s.Bills)
            .HasForeignKey(b => b.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Promocode)
            .WithMany(pr => pr.Bill)
            .HasForeignKey(b => b.PromocodeId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чеки на <0 Суммы
    }
}