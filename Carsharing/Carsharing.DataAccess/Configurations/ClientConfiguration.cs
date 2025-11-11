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
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cl => cl.UserId)
            .HasColumnName("client_user_id")
            .IsRequired();

        builder.Property(cl => cl.Name)
            .HasColumnName("client_name")
            .IsRequired();

        builder.Property(cl => cl.Surname)
            .HasColumnName("client_surname")
            .IsRequired(false);

        builder.Property(cl => cl.PhoneNumber)
            .HasColumnName("client_phone_number")
            .IsRequired();

        builder.Property(cl => cl.Email)
            .HasColumnName("client_email")
            .IsRequired();
    }
}