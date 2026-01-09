using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class BookingStatusConfiguration : IEntityTypeConfiguration<BookingStatusEntity>
{
    public void Configure(EntityTypeBuilder<BookingStatusEntity> builder)
    {
        builder.ToTable("booking_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new BookingStatusEntity
            {
                Id = (int)BookingStatusEnum.Active,
                Name = "Активно"
            },
            new BookingStatusEntity
            {
                Id = (int)BookingStatusEnum.Completed,
                Name = "Завершено"
            },
            new BookingStatusEntity
            {
                Id = (int)BookingStatusEnum.Cancelled,
                Name = "Отменено"
            });
    }
}