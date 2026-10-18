using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Document;

public class DocumentMinimalResponseDto
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
    public static Expression<Func<Models.Document, DocumentMinimalResponseDto>> MapDocumentMinimalResponseDto()
    {
        return document => new DocumentMinimalResponseDto
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

    public static DocumentMinimalResponseDto ToDocumentMinimalResponseDto(this Models.Document document)
    {
        return new DocumentMinimalResponseDto()
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