using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections;

public class AboutUsSectionsDeleterService(
    IAboutUsSectionsDeleterRepository aboutUsSectionsDeleterRepository,
    ILogger<AboutUsSectionsDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : IAboutUsSectionsDeleterService
{
    public async Task DeleteAboutUsSection(Guid id)
    {
        try
        {
            await aboutUsSectionsDeleterRepository.DeleteAboutUsSection(id);

            memoryCachesService.RemoveById(Constants.CACHE_ABOUT_US, id);
            memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

            logger.LogInformation("Sobre nosotros con Id: {Id} eliminado exitosamente.", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error eliminar  sobre nosotros con Id: {Id}", id);
            throw;
        }
    }
}