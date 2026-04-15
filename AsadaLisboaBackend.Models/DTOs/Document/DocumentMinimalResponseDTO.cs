using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Document
{
    public class DocumentMinimalResponseDTO
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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
                Url = document.Url,
                Slug = document.Slug,
                Title = document.Title,
                FileName = document.FileName,
                FilePath = document.FilePath,
                StatusId = document.StatusId,
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
                Url = document.Url,
                Slug = document.Slug,
                Title = document.Title,
                FileName = document.FileName,
                FilePath = document.FilePath,
                StatusId = document.StatusId,
                Description = document.Description,
                DocumentTypeName = document.DocumentType!.Name ?? "",
                Categories = document.Categories
                    .Select(c => c.Name)
                    .ToList(),
            };
        }
    }
}
