using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class NewConfiguration : IEntityTypeConfiguration<New>
    {
        public void Configure(EntityTypeBuilder<New> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(5000);
            builder.Property(x => x.PublicationDate).IsRequired();
            builder.Property(x => x.LastEditionDate).IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.HasIndex(x => x.PublicationDate);

            builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Categories)
                .WithMany(x => x.News)
                .UsingEntity(j => j.ToTable("NewsCategories"));
        }
    }
}
