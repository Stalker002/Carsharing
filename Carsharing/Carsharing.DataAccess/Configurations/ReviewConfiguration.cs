using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(x => x.Id);

        builder.Property(r => r.ClientId)
            .IsRequired();

        builder.Property(r => r.CarId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.Comment)
            .IsRequired();

        builder.Property(r => r.Date)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(r => r.Client)
            .WithMany(cl => cl.Reviews)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Car)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        //Можно сделать чек на рейтинг от 1 до 5
    }
}