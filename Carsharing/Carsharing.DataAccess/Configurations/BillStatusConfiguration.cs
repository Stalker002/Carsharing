using Carsharing.Core.Enum;
using Carsharing.Core.Validation;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class BillStatusConfiguration : IEntityTypeConfiguration<BillStatusEntity>
{
    public void Configure(EntityTypeBuilder<BillStatusEntity> builder)
    {
        builder.ToTable("bill_statuses");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Name)
            .HasMaxLength(ConstValidation.MaxNameLength)
            .IsRequired();

        builder.HasData(
            new BillStatusEntity
            {
                Id = (int)BillStatusEnum.Unpaid,
                Name = "Не оплачен"
            },
            new BillStatusEntity
            {
                Id = (int)BillStatusEnum.PartiallyPaid,
                Name = "Частично оплачен"
            },
            new BillStatusEntity
            {
                Id = (int)BillStatusEnum.Paid,
                Name = "Оплачен"
            },
            new BillStatusEntity
            {
                Id = (int)BillStatusEnum.Cancelled,
                Name = "Отменён"
            });
    }
}