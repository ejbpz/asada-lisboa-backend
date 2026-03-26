using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsUpdaterService : INewsUpdaterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemManager _fileSystem;
        private readonly INewsUpdaterRepository _newsUpdaterRepository;

        public NewsUpdaterService(INewsUpdaterRepository newsUpdaterRepository, ApplicationDbContext context, IFileSystemManager fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
            _newsUpdaterRepository = newsUpdaterRepository;
        }

        public async Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO)
        {
            // TODO: Update images and add principal
            //var imageUrl = Path.Combine(options.BasePath, id.ToString());

            //_imageService.UpdateImage

            var categories = await _context.Categories
                .Where(c => newRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();

            var slug = GenerateSlug.New(newRequestDTO.Title, id);
            //var filePath =
            //var fileName = 
            //var imageUrl = 

            var newModel = new New()
            {
                Id = id,
                Slug = slug,
                Title = newRequestDTO.Title,
                StatusId = newRequestDTO.StatusId,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
                Description = newRequestDTO.Description,
            };

            return (await _newsUpdaterRepository.UpdateNew(id, newModel))
                .ToNewResponseDTO();
        }
    }
}
