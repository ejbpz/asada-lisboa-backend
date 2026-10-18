using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Document;

public class DocumentResponseDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public long FileSize { get; set; }
    public DateTime PublicationDate { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;

    public Guid StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string DocumentTypeName { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = new();
}

public static partial class DocumentExtensions
{
    public static Expression<Func<Models.Document, DocumentResponseDto>> MapDocumentResponseDto()
    {
        return document => new DocumentResponseDto
        {
            Id = document.Id,
            StatusId = document.StatusId,
            Slug = document.Slug,
            Title = document.Title,
            FileName = document.FileName,
            FilePath = document.FilePath,
            FileSize = document.FileSize,
            Description = document.Description,
            PublicationDate = document.PublicationDate,                
            StatusName = document.Status!.Name ?? "",
            DocumentTypeName = document.DocumentType!.Name ?? "",
            Categories = document.Categories
                .Select(c => c.Name)
                .ToList(),
        };
    }

    public static DocumentResponseDto ToDocumentResponseDto(this Models.Document document)
    {
        return new DocumentResponseDto()
        {
            Id = document.Id,
            StatusId = document.StatusId,
            Slug = document.Slug,
            Title = document.Title,
            FileSize = document.FileSize,
            FileName = document.FileName,
            FilePath = document.FilePath,
            Description = document.Description,
            PublicationDate = document.PublicationDate,               
            StatusName = document.Status?.Name ?? "",
            DocumentTypeName = document.DocumentType?.Name ?? "",
            Categories = document.Categories
                .Select(c => c.Name)
                .ToList(),
        };
    }
}