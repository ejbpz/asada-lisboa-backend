using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Principal;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Principals;

namespace AsadaLisboaBackend.Services.Principals;

public class PrincipalsGetterService(
    INewsGetterService newsGetterService, 
    IImagesGetterService imagesGetterService, 
    IDocumentsGetterService documentsGetterService, 
    ILogger<PrincipalsGetterService> logger) 
    : IPrincipalsGetterService
{
    public async Task<PrincipalResponseDto> GetPrincipalInformation()
    {
        var searchSortRequestDto = new SearchSortRequestDto()
        {
            Take = 6,
            Offset = 0,
            IsPublic = true,
            SortDirection = "desc"
        };

        logger.LogInformation("Información principal obtenida correctamente.");

        return new PrincipalResponseDto()
        {
            News = (await newsGetterService.GetNews(searchSortRequestDto)).Data,
            Images = (await imagesGetterService.GetImages(searchSortRequestDto)).Data,
            Documents = (await documentsGetterService.GetDocuments(searchSortRequestDto)).Data
        };
    }

    public async Task<PrincipalResponseDto> GetPrincipalAdminInformation()
    {
        var searchSortRequestDto = new SearchSortRequestDto()
        {
            Take = 6,
            Offset = 0,
            IsPublic = false,
            SortDirection = "desc"
        };

        logger.LogInformation("Información principal administrativa obtenida correctamente.");

        return new PrincipalResponseDto()
        {
            News = (await newsGetterService.GetNews(searchSortRequestDto)).Data,
            Images = (await imagesGetterService.GetImages(searchSortRequestDto)).Data,
            Documents = (await documentsGetterService.GetDocuments(searchSortRequestDto)).Data
        };
    }
}