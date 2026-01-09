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

        builder.Property(p => p.BillId)
            .IsRequired();

        builder.Property(p => p.Sum)
            .IsRequired();

        builder.Property(p => p.Method)
            .HasDefaultValue("Картой")
            .IsRequired();

        builder.Property(p => p.Date)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(p => p.Bill)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}