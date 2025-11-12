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

        builder.Property(d => d.Id)
            .HasColumnName("document_id")
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(d => d.ClientId)
            .HasColumnName("document_client_id")
            .IsRequired();

        builder.Property(d => d.Type)
            .HasColumnName("document_type")
            .HasMaxLength(ClientDocument.MaxTypeLength)
            .IsRequired();

        builder.Property(d => d.Number)
            .HasColumnName("document_number")
            .HasMaxLength(ClientDocument.MaxNumberLength)
            .IsRequired();

        builder.Property(d => d.IssueDate)
            .HasColumnName("document_issue_date")
            .IsRequired();

        builder.Property(d => d.ExpiryDate)
            .HasColumnName("document_expiry_date")
            .IsRequired();

        builder.Property(d => d.FilePath)
            .HasColumnName("document_file_path")
            .HasMaxLength(ClientDocument.MaxFilePathLength)
            .IsRequired(false);

        builder.HasOne(d => d.Client)
            .WithMany(cl => cl.Documents)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}