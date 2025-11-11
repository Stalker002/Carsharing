using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class InsuranceConfiguration : IEntityTypeConfiguration<InsuranceEntity>
{
    public void Configure(EntityTypeBuilder<InsuranceEntity> builder)
    {
        builder.ToTable("fines");

        builder.HasKey(x => x.Id);

        builder.Property(i => i.Id)
            .HasColumnName("insurance_id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(i => i.CarId)
            .HasColumnName("insurance_car_id")
            .IsRequired();

        builder.Property(i => i.StatusId)
            .HasColumnName("insurance_status_id")
            .IsRequired();

        builder.Property(i => i.Type)
            .HasColumnName("insurance_type")
            .IsRequired();

        builder.Property(i => i.Company)
            .HasColumnName("insurance_company")
            .IsRequired();

        builder.Property(i => i.PolicyNumber)
            .HasColumnName("insurance_policy_number")
            .IsRequired();

        builder.Property(i => i.StartDate)
            .HasColumnName("insurance_start_name")
            .IsRequired();

        builder.Property(i => i.EndDate)
            .HasColumnName("insurance_end_date")
            .IsRequired();

        builder.Property(i => i.Cost)
            .HasColumnName("insurance_cost")
            .IsRequired();
    }
}