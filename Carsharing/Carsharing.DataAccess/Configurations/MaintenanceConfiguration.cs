using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class MaintenanceConfiguration : IEntityTypeConfiguration<MaintenanceEntity>
{
    public void Configure(EntityTypeBuilder<MaintenanceEntity> builder)
    {
        builder.ToTable("maintenance");

        builder.HasKey(x => x.Id);

        builder.Property(m => m.Id)
            .HasColumnName("maintenance_id");

        builder.Property(m => m.CarId)
            .HasColumnName("maintenance_car_id")
            .IsRequired();

        builder.Property(m => m.WorkType)
            .HasColumnName("maintenance_work_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasColumnName("maintenance_description")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(m => m.Cost)
            .HasColumnName("maintenance_cost")
            .IsRequired();

        builder.Property(m => m.Date)
            .HasColumnName("maintenance_date")
            .IsRequired();

        builder.HasOne(m => m.Car)
            .WithMany(c => c.Maintenance)
            .HasForeignKey(m => m.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable(t => t.HasCheckConstraint(
            "chk_maintenance_cost_nonneg",
            "maintenance_cost IS NULL OR maintenance_cost >= 0"));
    }
}