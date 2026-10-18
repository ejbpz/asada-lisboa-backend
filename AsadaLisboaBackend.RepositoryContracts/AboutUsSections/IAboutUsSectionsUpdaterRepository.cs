using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;

namespace AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

public interface IAboutUsSectionsUpdaterRepository
{
    public Task<AboutUsSection> UpdateAboutUsSection(Guid id, AboutUsRequestDto aboutUsRequestDto);
}