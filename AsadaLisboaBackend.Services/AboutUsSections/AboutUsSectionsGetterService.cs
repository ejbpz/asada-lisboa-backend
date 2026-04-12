using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsGetterService : IAboutUsSectionsGetterService
    {
        private readonly IAboutUsSectionsGetterRepository _aboutUsSectionsGetterRepository;
        private readonly ILogger<AboutUsSectionsGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

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
                searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

                var result = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(

                    resource: Constants.CACHE_USERS,
                    request: searchSortRequestDTO,
                    create: () => _aboutUsSectionsGetterRepository.GetAboutUsSections(searchSortRequestDTO),
                    time: TimeSpan.FromMinutes(5));

                _logger.LogInformation(
                    "Obtención exitosa de AboutUsSections. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener AboutUsSections. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );
                throw;
            }
        }
    }
}
