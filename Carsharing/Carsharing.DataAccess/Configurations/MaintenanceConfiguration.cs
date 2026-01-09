using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class MaintenanceConfiguration : IEntityTypeConfiguration<MaintenanceEntity>
{
    public void Configure(EntityTypeBuilder<MaintenanceEntity> builder)
    {
        builder.ToTable("maintenances");

        builder.HasKey(x => x.Id);

        builder.Property(m => m.CarId)
            .IsRequired();

        builder.Property(m => m.WorkType)
            .IsRequired();
        //Добавить enum

        builder.Property(m => m.Description)
            .IsRequired();

        builder.Property(m => m.Cost)
            .IsRequired();

        builder.Property(m => m.Date)
            .IsRequired();

        builder.HasOne(m => m.Car)
            .WithMany(c => c.Maintenance)
            .HasForeignKey(m => m.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        //Добавить чек на положительную сумму
    }
}