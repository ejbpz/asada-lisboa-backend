using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.New
{
    public class NewResponseDTO
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public DateTime LastEditionDate { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public string StatusName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class NewExtensions
    {
        public static Expression<Func<Models.New, NewResponseDTO>> MapNewResponseDTO()
        {
            return newModel => new NewResponseDTO
            {
                Id = newModel.Id,
                Slug = newModel.Slug,
                Title = newModel.Title,
                ImageUrl = newModel.ImageUrl,
                FilePath = newModel.FilePath,
                FileName = newModel.FileName,
                Description = newModel.Description,
                PublicationDate = newModel.PublicationDate,
                LastEditionDate = newModel.LastEditionDate,
                StatusName = newModel.Status!.Name ?? "",
                Categories = newModel.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static NewResponseDTO ToNewResponseDTO(this Models.New newModel)
        {
            return new NewResponseDTO()
            {
                Id = newModel.Id,
                Slug = newModel.Slug,
                Title = newModel.Title,
                ImageUrl = newModel.ImageUrl,
                FilePath = newModel.FilePath,
                FileName = newModel.FileName,
                Description = newModel.Description,
                PublicationDate = newModel.PublicationDate,
                LastEditionDate = newModel.LastEditionDate,
                StatusName = newModel.Status!.Name ?? "",
                Categories = newModel.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
