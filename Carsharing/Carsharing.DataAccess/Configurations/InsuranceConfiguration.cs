using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class InsuranceConfiguration : IEntityTypeConfiguration<InsuranceEntity>
{
    public void Configure(EntityTypeBuilder<InsuranceEntity> builder)
    {
        builder.ToTable("insurances");

        builder.HasKey(x => x.Id);

        builder.Property(i => i.CarId)
            .IsRequired();

        builder.Property(i => i.StatusId)
            .IsRequired();

        builder.Property(i => i.Type)
            .IsRequired();
        //Добавить enum

        builder.Property(i => i.Company)
            .HasMaxLength(Insurance.MaxCompanyLength)
            .IsRequired();

        builder.Property(i => i.PolicyNumber)
            .HasMaxLength(Insurance.MaxPolicyNumberLength)
            .IsRequired();

        builder.Property(i => i.StartDate)
            .IsRequired();

        builder.Property(i => i.EndDate)
            .IsRequired();

        builder.Property(i => i.Cost)
            .IsRequired();

        builder.HasIndex(i => i.PolicyNumber)
            .IsUnique();

        builder.HasOne(i => i.Car)
            .WithMany(c => c.Insurance)
            .HasForeignKey(i => i.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.InsuranceStatus)
            .WithMany(s => s.Insurances)
            .HasForeignKey(i => i.StatusId)
            .OnDelete(DeleteBehavior.SetNull);

        //Добавить чек на конец страховки не может быть перед началом
    }
}