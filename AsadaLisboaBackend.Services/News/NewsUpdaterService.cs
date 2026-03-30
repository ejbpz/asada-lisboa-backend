using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsUpdaterService : INewsUpdaterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemsManager _fileSystems;
        private readonly INewsGetterRepository _newsGetterRepository;
        private readonly IEditorsUpdaterService _editorsUpdaterService;
        private readonly IEditorsDeleterService _editorsDeleterService;
        private readonly INewsUpdaterRepository _newsUpdaterRepository;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsUpdaterService(INewsUpdaterRepository newsUpdaterRepository, INewsGetterRepository newsGetterRepository, IEditorsUpdaterService editorsUpdaterService, IEditorsDeleterService editorsDeleterService, IStatusesGetterRepository statusesGetterRepository, ApplicationDbContext context, IFileSystemsManager fileSystems)
        {
            _context = context;
            _fileSystems = fileSystems;
            _editorsDeleterService = editorsDeleterService;
            _editorsUpdaterService = editorsUpdaterService;
            _newsGetterRepository = newsGetterRepository;
            _newsUpdaterRepository = newsUpdaterRepository;
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

            var categories = await _context.Categories
                .Where(c => newRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();

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
