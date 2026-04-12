using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Services.MemoryCaches;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsDeleterService : IAboutUsSectionsDeleterService
    {
        private readonly IAboutUsSectionsDeleterRepository _aboutUsSectionsDeleterRepository;
        private readonly ILogger<AboutUsSectionsDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public AboutUsSectionsDeleterService(IAboutUsSectionsDeleterRepository aboutUsSectionsDeleterRepository, ILogger<AboutUsSectionsDeleterService> logger, IMemoryCachesService memoryCachesService)
        {
            _aboutUsSectionsDeleterRepository = aboutUsSectionsDeleterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task DeleteAboutUsSection(Guid id)
        {
            try
            {
                await _aboutUsSectionsDeleterRepository.DeleteAboutUsSection(id);

                _memoryCachesService.RemoveById(Constants.CACHE_ABOUT_US, id);
                _memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

                _logger.LogInformation("Sobre nosotros con Id: {Id} eliminado exitosamente.", id);
            }
            catch (Exception ex)
    {
                _logger.LogError(ex, "Error eliminar  sobre nosotros con Id: {Id}", id);
                throw;
            }
        }
    }
}
