using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carsharing.DataAccess.Configurations;

public class ClientDocumentConfiguration : IEntityTypeConfiguration<ClientDocumentEntity>
{
    public void Configure(EntityTypeBuilder<ClientDocumentEntity> builder)
    {
        builder.ToTable("client_documents");

        builder.HasKey(x => x.Id);

        builder.Property(d => d.ClientId)
            .IsRequired();

        builder.Property(d => d.Type)
            .HasMaxLength(ClientDocument.MaxTypeLength)
            .IsRequired();

        builder.Property(d => d.LicenseCategory)
            .HasMaxLength(ClientDocument.MaxLicenseCategoryLength)
            .IsRequired();

        builder.Property(d => d.Number)
            .HasMaxLength(ClientDocument.MaxNumberLength)
            .IsRequired();

        builder.Property(d => d.IssueDate)
            .IsRequired();

        builder.Property(d => d.ExpiryDate)
            .IsRequired();

        builder.Property(d => d.FilePath)
            .HasMaxLength(ClientDocument.MaxFilePathLength)
            .IsRequired(false);

        builder.HasOne(d => d.Client)
            .WithMany(cl => cl.Documents)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}