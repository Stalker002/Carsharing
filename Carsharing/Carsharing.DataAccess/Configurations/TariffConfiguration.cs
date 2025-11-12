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
            .HasColumnName("tariff_id")
            .IsRequired()
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasMaxLength(Tariff.MaxNameLength)
            .HasColumnName("tariff_name")
            .IsRequired();

        builder.Property(t => t.PricePerMinute)
            .HasColumnName("tariff_price_per_minute")
            .IsRequired();

        builder.Property(t => t.PricePerKm)
            .HasColumnName("tariff_price_per_km")
            .IsRequired();

        builder.Property(t => t.PricePerDay)
            .HasColumnName("tariff_price_per_day")
            .IsRequired();

        builder.HasMany(t => t.Cars)
            .WithOne(c => c.Tariff);

        //Чек на то что цена не ниже 0
    }
}