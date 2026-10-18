using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

public interface IAboutUsSectionsAdderRepository
{
    public Task<AboutUsSection> CreateAboutUsSection(AboutUsSection aboutUsSection);
}