using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class ChargeConfiguration : IEntityTypeConfiguration<Charge>
    {
        public void Configure(EntityTypeBuilder<Charge> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Charge { Id = Guid.NewGuid(), Name = "Presidente" },
                new Charge { Id = Guid.NewGuid(), Name = "Vicepresidente" },
                new Charge { Id = Guid.NewGuid(), Name = "Secretario" },
                new Charge { Id = Guid.NewGuid(), Name = "Tesorero" },
                new Charge { Id = Guid.NewGuid(), Name = "Vocal 1" },
                new Charge { Id = Guid.NewGuid(), Name = "Vocal 2" },
                new Charge { Id = Guid.NewGuid(), Name = "Vocal 3" },
                new Charge { Id = Guid.NewGuid(), Name = "Fiscal" });
        }
    }
}