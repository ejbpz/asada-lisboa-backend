using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsDeleterService : IAboutUsSectionsDeleterService
    {
        private readonly IAboutUsSectionsDeleterRepository _aboutUsSectionsDeleterRepository;
        private readonly ILogger<AboutUsSectionsDeleterService> _logger;

        public AboutUsSectionsDeleterService(IAboutUsSectionsDeleterRepository aboutUsSectionsDeleterRepository, ILogger<AboutUsSectionsDeleterService> logger)
        {
            _aboutUsSectionsDeleterRepository = aboutUsSectionsDeleterRepository;
            _logger = logger;
        }

        public async Task DeleteAboutUsSection(Guid id)
        {
            try
            {
                await _aboutUsSectionsDeleterRepository.DeleteAboutUsSection(id);

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
