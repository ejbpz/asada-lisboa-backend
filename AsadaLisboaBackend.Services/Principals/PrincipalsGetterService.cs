using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Principals;

namespace AsadaLisboaBackend.Services.Principals
{
    public class PrincipalsGetterService : IPrincipalsGetterService
    {
        private readonly INewsGetterService _newsGetterService;
        private readonly IImagesGetterService _imagesGetterService;
        private readonly IDocumentsGetterService _documentsGetterService;

        public PrincipalsGetterService(INewsGetterService newsGetterService, IImagesGetterService imagesGetterService, IDocumentsGetterService documentsGetterService)
        {
            _newsGetterService = newsGetterService;
            _imagesGetterService = imagesGetterService;
            _documentsGetterService = documentsGetterService;
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

            return new PrincipalRequestDTO()
            {
                News =  (await _newsGetterService.GetNews(searchSortRequestDTO)).Data,
                Images = (await _imagesGetterService.GetImages(searchSortRequestDTO)).Data,
                Documents = (await _documentsGetterService.GetDocuments(searchSortRequestDTO)).Data,
            };
        }
    }
}
