using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageMinimalResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class ImageExtensions
    {
        public static Expression<Func<Models.Image, ImageMinimalResponseDTO>> MapImageMinimalResponseDTO()
        {
            return image => new ImageMinimalResponseDTO
            {
                Id = image.Id,
                Url = image.Url,
                Slug = image.Slug,
                Title = image.Title,
                FileName = image.FileName,
                FilePath = image.FilePath,
                StatusId = image.StatusId,
                Description = image.Description,
                Categories = image.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static ImageMinimalResponseDTO ToImageMinimalResponseDTO(this Models.Image image)
        {
            return new ImageMinimalResponseDTO()
            {
                Id = image.Id,
                Url = image.Url,
                Slug = image.Slug,
                Title = image.Title,
                FileName = image.FileName,
                FilePath = image.FilePath,
                StatusId = image.StatusId,
                Description = image.Description,
                Categories = image.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}