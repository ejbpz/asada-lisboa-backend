using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsUpdaterService : INewsUpdaterService
    {
        private readonly INewsUpdaterRepository _newsUpdaterRepository;

        public NewsUpdaterService(INewsUpdaterRepository newsUpdaterRepository)
        {
            _newsUpdaterRepository = newsUpdaterRepository;
        }

        public async Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO, FileStorageOptions options)
        {
            // TODO: Update images and add principal

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

            return (await _newsUpdaterRepository.UpdateNew(id, newModel))
                .ToNewResponseDTO();
        }
    }
}
