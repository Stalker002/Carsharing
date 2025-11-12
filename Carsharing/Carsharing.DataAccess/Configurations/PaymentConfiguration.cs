using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
            .HasColumnName("payment_id")
            .ValueGeneratedOnAdd()
            .UseIdentityAlwaysColumn()
            .IsRequired();

        builder.Property(p => p.BillId)
            .HasColumnName("payment_bill_id")
            .IsRequired();

        builder.Property(p => p.Sum)
            .HasColumnName("payment_sum")
            .IsRequired();

        builder.Property(p => p.Method)
            .HasColumnName("payment_method")
            .HasDefaultValue("Картой")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasColumnName("payment_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(p => p.Bill)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}