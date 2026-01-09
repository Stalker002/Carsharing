using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class CarStatusConfiguration : IEntityTypeConfiguration<CarStatusEntity>
{
    public void Configure(EntityTypeBuilder<CarStatusEntity> builder)
    {
        builder.ToTable("car_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new CarStatusEntity
            {
                Id = (int)CarStatusEnum.Available,
                Name = "Доступен"
            },
            new CarStatusEntity
            {
                Id = (int)CarStatusEnum.Reserved,
                Name = "Недоступен"
            },
            new CarStatusEntity
            {
                Id = (int)CarStatusEnum.Maintenance,
                Name = "На обслуживании"
            },
            new CarStatusEntity
            {
                Id = (int)CarStatusEnum.Repair,
                Name = "В ремонте"
            });
    }
}