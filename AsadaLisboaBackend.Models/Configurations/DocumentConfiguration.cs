using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.PublicationDate).IsRequired();
            builder.Property(x => x.FileSize).IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.HasIndex(x => x.PublicationDate);

            builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DocumentType)
                .WithMany()
                .HasForeignKey(x => x.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Categories)
                .WithMany(x => x.Documents)
                .UsingEntity(j => j.ToTable("DocumentsCategories"));
        }
    }
}
