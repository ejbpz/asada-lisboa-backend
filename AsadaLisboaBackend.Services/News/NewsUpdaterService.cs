using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsUpdaterService : INewsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly INewsGetterRepository _newsGetterRepository;
        private readonly IEditorsUpdaterService _editorsUpdaterService;
        private readonly IEditorsDeleterService _editorsDeleterService;
        private readonly INewsUpdaterRepository _newsUpdaterRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsUpdaterService(INewsUpdaterRepository newsUpdaterRepository, INewsGetterRepository newsGetterRepository, IEditorsUpdaterService editorsUpdaterService, IEditorsDeleterService editorsDeleterService, IStatusesGetterRepository statusesGetterRepository, ICategoriesGetterService categoriesGetterService, IFileSystemsManager fileSystems)
        {
            _fileSystems = fileSystems;
            _newsGetterRepository = newsGetterRepository;
            _editorsDeleterService = editorsDeleterService;
            _editorsUpdaterService = editorsUpdaterService;
            _newsUpdaterRepository = newsUpdaterRepository;
            _categoriesGetterService = categoriesGetterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO)
        {
            var existingNew = await _newsGetterRepository.GetNew(id);

            var imageUrl = existingNew.ImageUrl;
            var fileName = existingNew.FileName;
            var filePath = existingNew.FilePath;

            if (newRequestDTO.File is not null)
            {
                var newImageUrl = await _fileSystems.SaveAsync(newRequestDTO.File, "news");

                if (!string.IsNullOrEmpty(existingNew.FileName))
                    await _fileSystems.DeleteAsync(existingNew.FileName, "news");

                imageUrl = newImageUrl;
                fileName = Path.GetFileName(imageUrl);
                filePath = $"news/{fileName}";
            }

            var content = await _editorsUpdaterService.ChangeHtmlImagesFolder(newRequestDTO.Description);
            await _editorsDeleterService.DeleteUnusedImages(existingNew.Description, newRequestDTO.Description);

            var categories = await _categoriesGetterService.ToCreateCategories(newRequestDTO.Categories);

            var status = await _statusesGetterRepository.GetStatus(newRequestDTO.StatusId);

            var newModel = new New()
            {
                Id = id,
                StatusId = status.Id,
                ImageUrl = imageUrl,
                FileName = fileName,
                FilePath = filePath,
                Description = content,
                Categories = categories,
                Slug = existingNew.Slug,
                Title = newRequestDTO.Title,
                LastEditionDate = DateTime.UtcNow,
                PublicationDate = existingNew.PublicationDate,
            };

            var created = await _newsUpdaterRepository.UpdateNew(id, newModel);

            if (created is null)
                throw new CreateObjectException("Error al crear la noticia.");

            return created.ToNewResponseDTO();
        }
    }
}
