using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Document
{
    public class DocumentResponseDTO
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public long FileSize { get; set; }
        public DateTime PublicationDate { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string DocumentTypeName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    public static partial class DocumentExtensions
    {
        public static Expression<Func<Models.Document, DocumentResponseDTO>> MapDocumentResponseDTO()
        {
            return document => new DocumentResponseDTO
            {
                Id = document.Id,
                PublicationDate = document.PublicationDate,                
                Slug = document.Slug,
                Title = document.Title,
                Description = document.Description,
                FileSize = document.FileSize,
                StatusName = document.Status!.Name ?? "",
                DocumentTypeName = document.DocumentType!.Name ?? "",
                Categories = document.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }

        public static DocumentResponseDTO ToDocumentResponseDTO(this Models.Document document)
        {
            return new DocumentResponseDTO()
            {
                Id = document.Id,
                PublicationDate = document.PublicationDate,               
                Slug = document.Slug,
                Title = document.Title,
                Description = document.Description,
                FileSize = document.FileSize,
                StatusName = document.Status!.Name ?? "",
                DocumentTypeName = document.DocumentType!.Name ?? "",
                Categories = document.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
