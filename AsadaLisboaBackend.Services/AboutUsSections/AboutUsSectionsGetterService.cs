using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsGetterService : IAboutUsSectionsGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ILogger<AboutUsSectionsGetterService> _logger;
        private readonly IAboutUsSectionsGetterRepository _aboutUsSectionsGetterRepository;

        public AboutUsSectionsGetterService(IAboutUsSectionsGetterRepository aboutUsSectionsGetterRepository, ILogger<AboutUsSectionsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _aboutUsSectionsGetterRepository = aboutUsSectionsGetterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<PageResponseDTO<AboutUsResponseDTO>> GetAboutUsSections(SearchSortRequestDTO searchSortRequestDTO)
        {
            try
            {
                var result = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(

                    resource: Constants.CACHE_USERS,
                    request: searchSortRequestDTO,
                    create: () => _aboutUsSectionsGetterRepository.GetAboutUsSections(searchSortRequestDTO),
                    time: TimeSpan.FromMinutes(5));

                _logger.LogInformation(
                    "Obtención exitosa de AboutUsSections. Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener AboutUsSections. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );
                throw;
            }
        }
    }
}
