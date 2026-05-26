using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsDeleterService : INewsDeleterService
    {
        private readonly ILogger<NewsDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly INewsGetterRepository _newsGetterRepository;
        private readonly INewsDeleterRepository _newsDeleterRepository;
        private readonly IEditorsDeleterService _editorsDeleterService;

        public NewsDeleterService(INewsDeleterRepository newsDeleterRepository, IEditorsDeleterService editorsDeleterService, INewsGetterRepository newsGetterRepository, ILogger<NewsDeleterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _newsGetterRepository = newsGetterRepository;
            _editorsDeleterService = editorsDeleterService;
            _newsDeleterRepository = newsDeleterRepository;
        }

        public async Task DeleteNew(Guid id)
        {
            var existingNew = await _newsGetterRepository.GetNew(id);

            if (existingNew is null)
            {
                _logger.LogError("Noticia con id: {Id} no encontrada.", id);
                throw new NotFoundException("La noticia no fue encontrada.");
            }

            await _editorsDeleterService.DeletePrincipalImage(existingNew.FileName);

            await _editorsDeleterService.DeleteContentImages(existingNew.Description);

            await _newsDeleterRepository.DeleteNew(id);

            _memoryCachesService.RemoveById(Constants.CACHE_NEWS, existingNew.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

            _logger.LogInformation("Noticia con id {Id} eliminada exitosamente.", id);
        }
    }
}
