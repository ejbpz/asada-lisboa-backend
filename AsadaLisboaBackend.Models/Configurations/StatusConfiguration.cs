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
                new Status { Id = Guid.NewGuid(), Name = "Draft" },
                new Status { Id = Guid.NewGuid(), Name = "Active" });
        }
    }
}
