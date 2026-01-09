using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class FineStatusConfiguration : IEntityTypeConfiguration<FineStatusEntity>
{
    public void Configure(EntityTypeBuilder<FineStatusEntity> builder)
    {
        builder.ToTable("fine_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new FineStatusEntity
            {
                Id = (int)FineStatusEnum.Issued,
                Name = "Начислен"
            },
            new FineStatusEntity
            {
                Id = (int)FineStatusEnum.PendingPayment,
                Name = "Ожидает оплаты"
            },
            new FineStatusEntity
            {
                Id = (int)FineStatusEnum.Paid,
                Name = "Оплачен"
            });
    }
}