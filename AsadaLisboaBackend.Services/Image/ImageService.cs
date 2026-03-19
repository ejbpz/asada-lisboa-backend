using AsadaLisboaBackend.ServiceContracts.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models;


namespace AsadaLisboaBackend.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO)
        {
           
            var newImage = new Image
            { 
                ID = Guid.NewGuid(),
                Title = imageRequestDTO.Title,
                Description = imageRequestDTO.Description,
                Slug = imageRequestDTO.Title.ToLower().Replace(" ", "-"),                
                PublicationDate = DateTime.UtcNow,
                FileSize = imageRequestDTO.FileSize,
                StatusId = imageRequestDTO.StatusId,
                Categories = _applicationDbContext.Categories
                .Where(c => imageRequestDTO.CategoryIds.Contains(c.Id))
                .ToList()
            };

            _applicationDbContext.Images.Add(newImage);
            await _applicationDbContext.SaveChangesAsync();

            return new ImageResponseDTO
            {
                Id = newImage.Id,
                Slug = newImage.Slug,
                Title = newImage.Title,
                Description = newImage.Description,
                PublicationDate = newImage.PublicationDate,
                FileSize = newImage.FileSize,
                StatusName = newImage.Status?.Name ?? string.Empty,
                Categories = newImage.Categories.Select(c => c.Name).ToList()
            };


        }

        public async Task<ImageResponseDTO> UpdateImage(ImageUpdateRequestDTO imageUpdateRequestDTO)
        {

        }

    }
}
