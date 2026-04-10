using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Principals;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Principals
{
    public class PrincipalsGetterService : IPrincipalsGetterService
    {
        private readonly INewsGetterService _newsGetterService;
        private readonly IImagesGetterService _imagesGetterService;
        private readonly IDocumentsGetterService _documentsGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;
        private readonly ILogger<PrincipalsGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public PrincipalsGetterService(INewsGetterService newsGetterService, IImagesGetterService imagesGetterService, IDocumentsGetterService documentsGetterService, IStatusesGetterRepository statusesGetterRepository, ILogger<PrincipalsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _newsGetterService = newsGetterService;
            _imagesGetterService = imagesGetterService;
            _documentsGetterService = documentsGetterService;
            _statusesGetterRepository = statusesGetterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<PrincipalRequestDTO> GetPrincipalInformation()
        {
            var searchSortRequestDTO = new SearchSortRequestDTO()
            {
                Page = 1,
                Take = 6,
                Offset = 0,
                Search = null,
                FilterBy = null,
                SortBy = "date",
                SortDirection = "asc",
            };


            var statusId = await _memoryCachesService.GetOrCreateCacheList<PrincipalRequestDTO>(

                   resource: Constants.CACHE_USERS,
                   request: searchSortRequestDTO,
                   create: () => _statusesGetterRepository.GetStatusPublicado(),
                   time: TimeSpan.FromMinutes(5));
            

            _logger.LogInformation("Información principal obtenida correctamente.");

            return new PrincipalRequestDTO()
            {
                News =  ((await _newsGetterService.GetNews(searchSortRequestDTO)).Data)
                            .Where(x => x.StatusId == statusId).ToList(),
                Images = ((await _imagesGetterService.GetImages(searchSortRequestDTO)).Data)
                            .Where(x => x.StatusId == statusId).ToList(),
                Documents = ((await _documentsGetterService.GetDocuments(searchSortRequestDTO)).Data)
                                .Where(x => x.StatusId == statusId).ToList(),
            };

            
        }
    }
}
