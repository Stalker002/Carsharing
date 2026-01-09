using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class PromocodeStatusConfiguration : IEntityTypeConfiguration<PromocodeStatusEntity>
{
    public void Configure(EntityTypeBuilder<PromocodeStatusEntity> builder)
    {
        builder.ToTable("promocode_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new PromocodeStatusEntity
            {
                Id = (int)PromocodeStatusEnum.Active,
                Name = "Активен"
            },
            new PromocodeStatusEntity
            {
                Id = (int)PromocodeStatusEnum.Expired,
                Name = "Истек"
            },
            new PromocodeStatusEntity
            {
                Id = (int)PromocodeStatusEnum.Used,
                Name = "Использован"
            });
    }
}