using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<StatusEntity>
{
    public void Configure(EntityTypeBuilder<StatusEntity> builder)
    {
        builder.ToTable("status");

        builder.HasKey(x => x.Id);

        builder.Property(st => st.Id)
            .HasColumnName("status_id")
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(st => st.Name)
            .HasColumnName("status_name")
            .HasMaxLength(Status.MaxNameLength)
            .IsRequired();

        builder.Property(st => st.Description)
            .HasColumnName("status_description")
            .IsRequired();

        builder.HasMany(s => s.Trip)
            .WithOne(tr => tr.Status);

        builder.HasMany(s => s.Bills)
            .WithOne(b => b.Status);

        builder.HasMany(s => s.Bookings)
            .WithOne(b => b.Status);

        builder.HasMany(s => s.Cars)
            .WithOne(c => c.Status);

        builder.HasMany(s => s.Fines)
            .WithOne(f => f.Status);

        builder.HasMany(s => s.Insurances)
            .WithOne(i => i.Status);

        builder.HasMany(s => s.Promocodes)
            .WithOne(p => p.Status);

    }
}