using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class TripStatusConfiguration : IEntityTypeConfiguration<TripStatusEntity>
{
    public void Configure(EntityTypeBuilder<TripStatusEntity> builder)
    {
        builder.ToTable("trip_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new TripStatusEntity
            {
                Id = (int)TripStatusEnum.WaitingStart,
                Name = "Ожидание начала"
            },
            new TripStatusEntity
            {
                Id = (int)TripStatusEnum.EnRoute,
                Name = "В пути"
            },
            new TripStatusEntity
            {
                Id = (int)TripStatusEnum.Finished,
                Name = "Завершена"
            },
            new TripStatusEntity
            {
                Id = (int)TripStatusEnum.Cancelled,
                Name = "Отменена"
            },
            new TripStatusEntity
            {
                Id = (int)TripStatusEnum.PaymentRequired,
                Name = "Требуется оплата"
            });
    }
}