using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.New
{
    public class NewMinimalResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public DateTime LastEditionDate { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class NewExtensions
    {
        public static Expression<Func<Models.New, NewMinimalResponseDTO>> MapNewMinimalResponseDTO()
        {
            return newModel => new NewMinimalResponseDTO
            {
                Id = newModel.Id,
                Slug = newModel.Slug,
                Title = newModel.Title,
                FileName = newModel.FileName,
                FilePath = newModel.FilePath,
                ImageUrl = newModel.ImageUrl,
                StatusId = newModel.StatusId,
                Description = newModel.Description,
                LastEditionDate = newModel.LastEditionDate,
                Categories = newModel.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static NewMinimalResponseDTO ToNewMinimalResponseDTO(this Models.New newModel)
        {
            return new NewMinimalResponseDTO()
            {
                Id = newModel.Id,
                Slug = newModel.Slug,
                Title = newModel.Title,
                ImageUrl = newModel.ImageUrl,
                FileName = newModel.FileName,
                FilePath = newModel.FilePath,
                StatusId = newModel.StatusId,
                Description = newModel.Description,
                LastEditionDate = newModel.LastEditionDate,
                Categories = newModel.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
