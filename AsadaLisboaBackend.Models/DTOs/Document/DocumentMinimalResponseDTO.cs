using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Document
{
    public class DocumentMinimalResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public long FileSize { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class DocumentExtensions
    {
        public static Expression<Func<Models.Document, DocumentMinimalResponseDTO>> MapDocumentMinimalResponseDTO()
        {
            return document => new DocumentMinimalResponseDTO
            {
                Id = document.Id,
                Slug = document.Slug,
                Title = document.Title,
                StatusId = document.StatusId,
                FileSize = document.FileSize,
                Description = document.Description,
                DocumentTypeName = document.DocumentType!.Name ?? "",
                Categories = document.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static DocumentMinimalResponseDTO ToDocumentMinimalResponseDTO(this Models.Document document)
        {
            return new DocumentMinimalResponseDTO()
            {
                Id = document.Id,
                Slug = document.Slug,
                Title = document.Title,
                StatusId = document.StatusId,
                FileSize = document.FileSize,
                Description = document.Description,
                DocumentTypeName = document.DocumentType!.Name ?? "",
                Categories = document.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
