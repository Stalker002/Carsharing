using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder.ToTable("review");

        builder.HasKey(x => x.Id);

        builder.Property(r => r.Id)
            .HasColumnName("review_id")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(r => r.ClientId)
            .HasColumnName("review_client_id")
            .IsRequired();

        builder.Property(r => r.CarId)
            .HasColumnName("review_car_id")
            .IsRequired();

        builder.Property(r => r.Rating)
            .HasColumnName("review_rating")
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasColumnName("review_comment")
            .IsRequired();

        builder.Property(r => r.Date)
            .HasColumnName("review_date")
            .IsRequired();
    }
}