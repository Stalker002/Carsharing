using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.ToTable("clients");

        builder.HasKey(x => x.Id);

        builder.Property(cl => cl.Id)
            .HasColumnName("client_id")
            .UseIdentityAlwaysColumn()
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cl => cl.UserId)
            .HasColumnName("client_user_id")
            .IsRequired();

        builder.Property(cl => cl.Name)
            .HasColumnName("client_name")
            .HasMaxLength(Client.MaxNameLength)
            .IsRequired();

        builder.Property(cl => cl.Surname)
            .HasColumnName("client_surname")
            .HasMaxLength(Client.MaxSurnameLength)
            .IsRequired(false);

        builder.Property(cl => cl.PhoneNumber)
            .HasColumnName("client_phone_number")
            .HasMaxLength(Client.MaxPhoneLength)
            .IsRequired();

        builder.Property(cl => cl.Email)
            .HasColumnName("client_email")
            .HasMaxLength(Client.MaxEmailLength)
            .IsRequired();

        builder.HasIndex(cl => cl.Email)
            .IsUnique();

        builder.HasIndex(cl => cl.PhoneNumber)
            .IsUnique();
        
       builder.HasIndex(cl => cl.UserId)
            .IsUnique();

        builder.HasOne(cl => cl.User)
            .WithOne(u => u.Client)
            .HasForeignKey<ClientEntity>(cl => cl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(cl => cl.Documents)
            .WithOne(d => d.Client);

        builder.HasMany(cl => cl.Reviews)
            .WithOne(d => d.Client);

        builder.HasMany(cl => cl.Bookings)
            .WithOne(d => d.Client);

        //Можно сделать чек на валидацию email
    }
}