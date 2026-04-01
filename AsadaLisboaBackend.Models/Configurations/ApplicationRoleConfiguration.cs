using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole { Id = Guid.Parse("864C6F86-5FDF-4279-857B-D7B8D99170B2"), Name = "Administrador", NormalizedName = "ADMINISTRADOR", ConcurrencyStamp = "864C6F86-5FDF-4279-857B-D7B8D99170B2" },
                new ApplicationRole { Id = Guid.Parse("6FCF3478-3CED-400B-92A6-DBE6A9D37446"), Name = "Escritor", NormalizedName = "ESCRITOR", ConcurrencyStamp = "6FCF3478-3CED-400B-92A6-DBE6A9D37446" },
                new ApplicationRole { Id = Guid.Parse("CAA2CBCD-3A76-4ED0-AB4F-7D07034692EB"), Name = "Lector", NormalizedName = "LECTOR", ConcurrencyStamp = "CAA2CBCD-3A76-4ED0-AB4F-7D07034692EB" });
        }
    }
}
