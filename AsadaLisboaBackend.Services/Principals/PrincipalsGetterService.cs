using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Principals;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Principals
{
    public class PrincipalsGetterService : IPrincipalsGetterService
    {
        private readonly INewsGetterService _newsGetterService;
        private readonly ILogger<PrincipalsGetterService> _logger;
        private readonly IImagesGetterService _imagesGetterService;
        private readonly IDocumentsGetterService _documentsGetterService;

        public PrincipalsGetterService(INewsGetterService newsGetterService, IImagesGetterService imagesGetterService, IDocumentsGetterService documentsGetterService, IStatusesGetterRepository statusesGetterRepository, ILogger<PrincipalsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _newsGetterService = newsGetterService;
            _imagesGetterService = imagesGetterService;
            _documentsGetterService = documentsGetterService;
        }

        public async Task<PrincipalRequestDTO> GetPrincipalInformation()
        {
            var searchSortRequestDTO = new SearchSortRequestDTO()
            {
                Take = 6,
                Offset = 0,
                IsPublic = true,
            };

            _logger.LogInformation("Información principal obtenida correctamente.");

            return new PrincipalRequestDTO()
            {
                News = (await _newsGetterService.GetNews(searchSortRequestDTO)).Data,
                Images = (await _imagesGetterService.GetImages(searchSortRequestDTO)).Data,
                Documents = (await _documentsGetterService.GetDocuments(searchSortRequestDTO)).Data
            };
        }
    }
}
