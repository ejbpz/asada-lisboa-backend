using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News;

public class NewsDeleterService(
    INewsDeleterRepository newsDeleterRepository,
    IEditorsDeleterService editorsDeleterService,
    INewsGetterRepository newsGetterRepository,
    ILogger<NewsDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : INewsDeleterService
{
    public async Task DeleteNew(Guid id)
    {
        var existingNew = await newsGetterRepository.GetNew(id);

        await editorsDeleterService.DeletePrincipalImage(existingNew.FileName);
        await editorsDeleterService.DeleteContentImages(existingNew.Description);
        await newsDeleterRepository.DeleteNew(id);

        memoryCachesService.RemoveById(Constants.CACHE_NEWS, existingNew.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

        logger.LogInformation("Noticia con id {Id} eliminada exitosamente.", id);
    }
}