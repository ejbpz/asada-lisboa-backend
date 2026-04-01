using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Extension)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.MimeType)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Extension)
                .IsUnique();

            builder.HasData(
                new DocumentType { Id = Guid.Parse("D8FBE9B6-1A5E-41EA-98EA-E6641A6047C8"), Name = "PDF", Extension = ".pdf", MimeType = "application/pdf" },
                new DocumentType { Id = Guid.Parse("9029EB69-0E77-4CF6-8622-C7902D565745"), Name = "Word", Extension = ".docx", MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                new DocumentType { Id = Guid.Parse("1E42A6E9-0BAA-4C51-8812-E459E0678130"), Name = "Excel", Extension = ".xlsx", MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                new DocumentType { Id = Guid.Parse("55931B3A-AE2B-4AEC-8625-7C9ADBDFAB5F"), Name = "CSV", Extension = ".csv", MimeType = "text/csv" },
                new DocumentType { Id = Guid.Parse("D9EC3706-86F8-4C3D-A770-E3E38221A7E8"), Name = "Texto", Extension = ".txt", MimeType = "text/plain" },
                new DocumentType { Id = Guid.Parse("F4365613-D7E7-488E-B24E-4834645408D6"), Name = "ZIP", Extension = ".zip", MimeType = "application/octet-stream" });
        }
    }
}
