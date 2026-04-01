using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Category { Id = Guid.Parse("03E622EC-438B-49F3-8C53-3A3873651C16"), Name = "Proyectos Ejecutados" },
                new Category { Id = Guid.Parse("6DA5CF2D-E946-43ED-A092-D5B4C9282FFA"), Name = "Estados Financieros" }, 
                new Category { Id = Guid.Parse("3643CD9F-99FE-4873-85F9-2A7B6B4DAC28"), Name = "Tanque Principal" },
                new Category { Id = Guid.Parse("F62ED863-70D4-446B-9088-10EFD6CB6C77"), Name = "Pozo Principal" },
                new Category { Id = Guid.Parse("77607C99-2ED1-4B8E-89CC-CCC7C6344E53"), Name = "Lineamientos" },
                new Category { Id = Guid.Parse("2B3B492D-E243-4FC8-8217-03EA9998A201"), Name = "Sugerencias" },
                new Category { Id = Guid.Parse("BA84F8AC-EAD7-4CB3-83B4-77238DF95884"), Name = "Solicitudes" },
                new Category { Id = Guid.Parse("93CA9020-78FD-4080-8D57-469EB05D9DC3"), Name = "Reglamentos" },
                new Category { Id = Guid.Parse("783E94BC-4748-4223-A150-8892354B865B"), Name = "Colindancia" },
                new Category { Id = Guid.Parse("560841FB-884B-45F6-9A09-18D4641B7AF5"), Name = "Hidrantes" },
                new Category { Id = Guid.Parse("5AA04636-CD02-4184-BBE0-603A2E785988"), Name = "Convenios" },
                new Category { Id = Guid.Parse("81F11C15-3556-40BE-8074-A6BE7A5D5AB4"), Name = "Informes" },
                new Category { Id = Guid.Parse("CEB0BB8A-0414-474C-B378-46491B4C08E8"), Name = "Exámenes" },
                new Category { Id = Guid.Parse("B2DBCF36-FDDF-4752-B39D-B31F61E704A3"), Name = "Estudios" },
                new Category { Id = Guid.Parse("71B9C756-B148-4E0F-A5C7-09A8A7AA209E"), Name = "Dudas" });
        }
    }
}
