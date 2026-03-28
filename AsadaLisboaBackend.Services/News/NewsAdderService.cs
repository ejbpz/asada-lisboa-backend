using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.FileSystem;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemManager _fileSystem;
        private readonly INewsAdderRepository _newsAdderRepository;
        private readonly IEditorUpdaterService _editorUpdaterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsAdderService(INewsAdderRepository newsAdderRepository, IEditorUpdaterService editorUpdaterService, IStatusesGetterRepository statusesGetterRepository, ApplicationDbContext context, IFileSystemManager fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
            _newsAdderRepository = newsAdderRepository;
            _editorUpdaterService = editorUpdaterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO)
        {
            var id = Guid.NewGuid();

            string imageUrl = string.Empty;
            string fileName = string.Empty;
            string filePath = string.Empty;

            if (newRequestDTO.File is not null)
            {
                imageUrl = await _fileSystem.SaveAsync(newRequestDTO.File, "news");
                fileName = Path.GetFileName(imageUrl);
            }

            if(imageUrl != string.Empty && fileName != string.Empty)
                filePath = $"news/{fileName}";

            var content = await _editorUpdaterService.ChangeHtmlImagesFolder(newRequestDTO.Description);

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
                Title = newRequestDTO.Title,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
                Slug = GenerateSlug.New(newRequestDTO.Title, id),
            };

            var created = await _newsAdderRepository.CreateNew(newModel);

            if (created is null)
                throw new CreateObjectException("Error al crear la noticia.");

            return created.ToNewResponseDTO();
        }
    }
}
