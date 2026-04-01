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
                new Charge { Id = Guid.Parse("0FEB0B33-CF81-4973-9754-E45DE997069D"), Name = "Administrador" },
                new Charge { Id = Guid.Parse("7D591140-46FA-47A3-9875-D450896C8B14"), Name = "Vicepresidente" },
                new Charge { Id = Guid.Parse("56F6B0C4-9E67-4CB6-AC31-D95AC0D10271"), Name = "Presidente" },
                new Charge { Id = Guid.Parse("1BCE957A-451A-425A-B438-58C0227FF9FC"), Name = "Secretario" },
                new Charge { Id = Guid.Parse("C8FCB329-DCAF-4352-80FC-5CC7730482E7"), Name = "Tesorero" },
                new Charge { Id = Guid.Parse("9DB79916-BAC2-479A-84A1-DFB13C83DDA8"), Name = "Vocal 1" },
                new Charge { Id = Guid.Parse("64849CA6-6DAA-4174-A240-A640CD09509F"), Name = "Vocal 2" },
                new Charge { Id = Guid.Parse("28524859-89FD-4F06-A39C-C62150C1284C"), Name = "Vocal 3" },
                new Charge { Id = Guid.Parse("BEE47254-066C-414B-A872-3CC72D708A67"), Name = "Fiscal" });
        }
    }
}