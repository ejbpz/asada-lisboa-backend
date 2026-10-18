using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Repositories.AboutUsSections;

public class AboutUsSectionsAdderRepository(ApplicationDbContext context) : IAboutUsSectionsAdderRepository
{
    public async Task<AboutUsSection> CreateAboutUsSection(AboutUsSection aboutUsSection)
    {
        context.AboutUsSections.Add(aboutUsSection);
        var affectedRows = await context.SaveChangesAsync();

        if (affectedRows < 1)
            throw new NotFoundException("Error al crear la sección.");

        return aboutUsSection;
    }
}