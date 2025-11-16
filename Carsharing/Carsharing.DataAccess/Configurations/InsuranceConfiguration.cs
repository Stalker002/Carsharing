using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class InsuranceConfiguration : IEntityTypeConfiguration<InsuranceEntity>
{
    public void Configure(EntityTypeBuilder<InsuranceEntity> builder)
    {
        builder.ToTable("insurance");

        builder.HasKey(x => x.Id);

        builder.Property(i => i.Id)
            .HasColumnName("insurance_id")
            .UseIdentityAlwaysColumn()
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
        //Добавить enum

        builder.Property(i => i.Company)
            .HasColumnName("insurance_company")
            .HasMaxLength(Insurance.MaxCompanyLength)
            .IsRequired();

        builder.Property(i => i.PolicyNumber)
            .HasColumnName("insurance_policy_number")
            .HasMaxLength(Insurance.MaxPolicyNumberLength)
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

        builder.HasIndex(i => i.PolicyNumber)
            .IsUnique();

        builder.HasOne(i => i.Car)
            .WithMany(c => c.Insurance)
            .HasForeignKey(i => i.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Status)
            .WithMany(s => s.Insurances)
            .HasForeignKey(i => i.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Добавить чек на конец страховки не может быть перед началом
    }
}