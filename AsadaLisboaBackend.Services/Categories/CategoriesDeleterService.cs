using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories;

public class CategoriesDeleterService(
    ICategoriesDeleterRepository categoriesDeleterRepository,
    ILogger<CategoriesDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : ICategoriesDeleterService
{
    public async Task DeleteCategory(Guid id)
    {
        await categoriesDeleterRepository.DeleteCategory(id);
        logger.LogInformation("Categor ía con id {Id} borrado exitosamente.", id);

        memoryCachesService.RemoveById(Constants.CACHE_CATEGORIES, id);
        memoryCachesService.ChangeVersion(Constants.CACHE_CATEGORIES);
    }
}