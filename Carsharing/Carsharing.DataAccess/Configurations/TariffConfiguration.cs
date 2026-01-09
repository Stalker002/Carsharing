using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class TariffConfiguration : IEntityTypeConfiguration<TariffEntity>
{
    public void Configure(EntityTypeBuilder<TariffEntity> builder)
    {
        builder.ToTable("tariffs");

        builder.HasKey(x => x.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(Tariff.MaxNameLength)
            .IsRequired();

        builder.Property(t => t.PricePerMinute)
            .IsRequired();

        builder.Property(t => t.PricePerKm)
            .IsRequired();

        builder.Property(t => t.PricePerDay)
            .IsRequired();

        builder.HasMany(t => t.Cars)
            .WithOne(c => c.Tariff);

        //Чек на то что цена не ниже 0
    }
}