using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class InsuranceStatusConfiguration : IEntityTypeConfiguration<InsuranceStatusEntity>
{
    public void Configure(EntityTypeBuilder<InsuranceStatusEntity> builder)
    {
        builder.ToTable("insurance_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new InsuranceStatusEntity
            {
                Id = (int)InsuranceStatusEnum.Active,
                Name = "Активна"
            },
            new InsuranceStatusEntity
            {
                Id = (int)InsuranceStatusEnum.Expired,
                Name = "Истекла"
            },
            new InsuranceStatusEntity
            {
                Id = (int)InsuranceStatusEnum.Annulled,
                Name = "Аннулирована"
            });
    }
}