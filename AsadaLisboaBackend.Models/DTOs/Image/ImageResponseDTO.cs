using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageResponseDTO
    {
        public Guid Id { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string StatusName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class ImageExtensions
    {
        public static Expression<Func<Models.Image, ImageResponseDTO>> MapImageResponseDTO()
        {
            return image => new ImageResponseDTO
            {
                Id = image.Id,
                Url = image.Url,
                Slug = image.Slug,
                Title = image.Title,
                FilePath = image.FilePath,
                FileName = image.FileName,
                FileSize = image.FileSize,
                Description = image.Description,
                PublicationDate = image.PublicationDate,
                StatusName = image.Status!.Name ?? "Pendiente",
                Categories = image.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static ImageResponseDTO ToImageResponseDTO(this Models.Image image)
        {
            return new ImageResponseDTO()
            {
                Id = image.Id,
                Url = image.Url,
                Slug = image.Slug,
                Title = image.Title,
                FilePath = image.FilePath,
                FileName = image.FileName,
                FileSize = image.FileSize,
                Description = image.Description,
                PublicationDate = image.PublicationDate,
                StatusName = image.Status!.Name ?? "Pendiente",
                Categories = image.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
