using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemsManager _fileSystems;
        private readonly INewsAdderRepository _newsAdderRepository;
        private readonly IEditorsUpdaterService _editorsUpdaterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsAdderService(INewsAdderRepository newsAdderRepository, IEditorsUpdaterService editorsUpdaterService, IStatusesGetterRepository statusesGetterRepository, ApplicationDbContext context, IFileSystemsManager fileSystems)
        {
            _context = context;
            _fileSystems = fileSystems;
            _newsAdderRepository = newsAdderRepository;
            _editorsUpdaterService = editorsUpdaterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO)
        {
            var id = Guid.NewGuid();

            string imageUrl = string.Empty;
            string fileName = string.Empty;
            string filePath = string.Empty;

            if (newRequestDTO.File is not null && newRequestDTO.File.Length > 0)
            {
                imageUrl = await _fileSystems.SaveAsync(newRequestDTO.File, "news");
                fileName = Path.GetFileName(imageUrl);
            }

            if(imageUrl != string.Empty && fileName != string.Empty)
                filePath = $"news/{fileName}";

            var content = await _editorsUpdaterService.ChangeHtmlImagesFolder(newRequestDTO.Description);

            //var categories = await _context.Categories
            //    .Where(c => newRequestDTO.CategoryIds.Contains(c.Id))
            //    .ToListAsync();

            var status = await _statusesGetterRepository.GetStatus(newRequestDTO.StatusId);

            var newModel = new New()
            {
                Id = id,
                StatusId = status.Id,
                ImageUrl = imageUrl,
                FileName = fileName,
                FilePath = filePath,
                Description = content,
                //Categories = categories,
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
