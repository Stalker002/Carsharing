using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class FavoritesConfiguration : IEntityTypeConfiguration<FavoritesEntity>
{
    public void Configure(EntityTypeBuilder<FavoritesEntity> builder)
    {
        builder.ToTable("favorite_cars");

        builder.HasKey(x => x.Id);

        builder.Property(f => f.ClientId)
            .IsRequired();

        builder.Property(f => f.CarId)
            .IsRequired();

        builder.HasOne(r => r.Client)
            .WithMany(cl => cl.Favorites)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Car)
            .WithMany(c => c.Favorites)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}