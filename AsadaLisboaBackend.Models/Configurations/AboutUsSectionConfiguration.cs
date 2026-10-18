using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsadaLisboaBackend.Models.Configurations;

public class AboutUsSectionConfiguration : IEntityTypeConfiguration<AboutUsSection>
{
    public void Configure(EntityTypeBuilder<AboutUsSection> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SectionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(2500);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.HasIndex(x => x.Order);

        builder.HasData(
            new AboutUsSection { Id = Guid.Parse("71DADBE4-7815-4994-A8DA-7AE6154769DF"), SectionType = "Historia", Content = "La ASADA Urbanización Lisboa surge como una iniciativa comunal ante la necesidad de administrar de forma organizada el sistema de abastecimiento de agua potable de la comunidad. Con el crecimiento de la población y el aumento en la demanda del servicio, los vecinos se organizaron para garantizar una distribución eficiente, segura y continua del recurso hídrico.\nCon el paso del tiempo, la organización ha fortalecido su infraestructura mediante la implementación de pozos, tanques de almacenamiento y redes de distribución, así como el desarrollo de procesos administrativos y financieros que permiten la sostenibilidad del servicio. En la actualidad, la ASADA continúa enfocada en la mejora continua, promoviendo el uso responsable del agua y la participación activa de la comunidad.", Order = 1 },
            new AboutUsSection { Id = Guid.Parse("E8FA8E42-A5A2-4D94-8110-613F639AD896"), SectionType = "Misión", Content = "Brindar el servicio de agua potable a la comunidad de Urbanización Lisboa, garantizando altos estándares de calidad, continuidad y cobertura, mediante una gestión responsable y eficiente de los recursos hídricos y financieros.\nLa organización procura el adecuado mantenimiento, mejora y expansión de la infraestructura del acueducto, asegurando la sostenibilidad del servicio en el tiempo. Asimismo, promueve una atención oportuna y transparente a los abonados, contribuyendo a la salud pública, el bienestar social y el desarrollo integral de la comunidad, bajo principios de equidad, responsabilidad y compromiso ambiental.", Order = 2 },
            new AboutUsSection { Id = Guid.Parse("02F9CF72-83FD-4DF0-BA40-8FD8AE2E9C8E"), SectionType = "Visión", Content = "Ser una organización comunal líder en la gestión del recurso hídrico a nivel local, reconocida por su eficiencia operativa, transparencia administrativa y compromiso con la sostenibilidad ambiental. La ASADA Urbanización Lisboa aspira a consolidarse como un modelo de referencia en la administración de servicios de agua potable, incorporando mejoras continuas en sus procesos, tecnología e infraestructura. Busca garantizar el acceso seguro y sostenible al agua para las generaciones presentes y futuras, fortaleciendo la participación comunitaria y fomentando una cultura de uso responsable del recurso hídrico.", Order = 3 },
            new AboutUsSection { Id = Guid.Parse("8C764A82-E07B-4F86-BE81-DBB7A4478A7E"), SectionType = "Funciones", Content = "La ASADA Urbanización Lisboa se encarga de administrar, operar y mantener el sistema de acueducto comunal, asegurando la captación, almacenamiento y distribución del agua potable hacia los usuarios. Asimismo, gestiona la facturación y el cobro del servicio, brinda atención a los abonados, ejecuta labores de mantenimiento preventivo y correctivo en la infraestructura, y vela por la calidad del agua suministrada.\nParalelamente, cumple funciones administrativas y financieras, incluyendo la elaboración de informes, la organización de asambleas y el cumplimiento de regulaciones vigentes, además de promover el uso responsable del recurso hídrico y la protección del entorno ambiental.", Order = 4 });
    }
}