using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.New
{
    public class NewMinimalResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
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
                ImageUrl = newModel.ImageUrl,
                StatusId = newModel.StatusId,
                Description = newModel.Description,
                PublicationDate = newModel.PublicationDate,
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
                StatusId = newModel.StatusId,
                Description = newModel.Description,
                PublicationDate = newModel.PublicationDate,
                Categories = newModel.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
