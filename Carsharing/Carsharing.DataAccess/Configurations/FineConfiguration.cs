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

        builder.Property(f => f.TripId)
            .IsRequired();

        builder.Property(f => f.StatusId)
            .IsRequired();

        builder.Property(f => f.Type)
            .IsRequired();

        builder.Property(f => f.Amount)
            .IsRequired();

        builder.Property(f => f.Date)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_DATE");

        builder.HasOne(f => f.Trip)
            .WithMany(tr => tr.Fine)
            .HasForeignKey(f => f.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.FineStatus)
            .WithMany(s => s.Fines)
            .HasForeignKey(f => f.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Чек на не <0 стоимость
    }
}