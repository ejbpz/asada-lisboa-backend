using AsadaLisboaBackend.Models.DTOs.AboutUs;

namespace AsadaLisboaBackend.ServiceContracts.AboutUsSections;

public interface IAboutUsSectionsUpdaterService
{
    public Task<AboutUsResponseDto> UpdateAboutUsSection(Guid id, AboutUsRequestDto aboutUsRequestDto);
}