using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ContactType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasIndex(x => x.Order);

        builder.HasData(
            new Contact { Id = Guid.Parse("0AC0C004-B812-4F29-855F-5813E89146D9"), ContactType = "Correo electrónico", Value = "asadaurblisboa@gmail.com", Order = 1 },
            new Contact { Id = Guid.Parse("CEE070C5-4FA6-40A3-AE12-DB5F2EA8BAD1"), ContactType = "Teléfono Celular", Value = "+506 8459-5494", Order = 2 },
            new Contact { Id = Guid.Parse("A78F2955-59B4-4C88-A429-B9644B19552C"), ContactType = "Teléfono Fijo", Value = "+506 2433-3882", Order = 3 },
            new Contact { Id = Guid.Parse("491D6C63-98F0-44F9-B837-FF9640D01921"), ContactType = "Ubicación", Value = "https://maps.app.goo.gl/7vArCkz5iEG5abq48", Order = 4 },
            new Contact { Id = Guid.Parse("37DD984F-7059-48AE-A6D5-CF57D8C2667B"), ContactType = "Facebook", Value = "https://www.facebook.com/asadalisboa/", Order = 5 },
            new Contact { Id = Guid.Parse("E0B3DF15-177C-4581-8FC0-944B205D3B3A"), ContactType = "Horario", Value = "Lunes a viernes: 8:00 a.m. - 12:00 p.m.", Order = 6 });
    }
}