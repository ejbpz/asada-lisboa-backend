using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Status { Id = Guid.Parse("2C36A4B8-DE3E-4607-9B7B-DBE0F0E00390"), Name = "Borrador" },
                new Status { Id = Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"), Name = "Publicado" });
        }
    }
}
