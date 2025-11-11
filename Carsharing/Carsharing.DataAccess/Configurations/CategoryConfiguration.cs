using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("category_id")
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("category_name")
            .IsRequired();
    }
}