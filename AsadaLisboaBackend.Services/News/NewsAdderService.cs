using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly INewsAdderRepository _newsAdderRepository;

        public NewsAdderService(INewsAdderRepository newsAdderRepository)
        {
            _newsAdderRepository = newsAdderRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO, FileStorageOptions fileStorageOptions)
        {
            var id = Guid.NewGuid();

            // TODO: Add principal image and content images

            //var categories = await _
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

            return (await _newsAdderRepository.CreateNew(newModel))
                .ToNewResponseDTO();
        }
    }
}
