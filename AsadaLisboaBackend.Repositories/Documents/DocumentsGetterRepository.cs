using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;


namespace AsadaLisboaBackend.Repositories.Documents;

public class DocumentsGetterRepository(ApplicationDbContext context) : IDocumentsGetterRepository
{
    public async Task<PageResponseDto<DocumentMinimalResponseDto>> GetDocuments(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<Document> query = context.Documents
            .AsNoTracking()
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .Include(d => d.DocumentType);

        if (searchSortRequestDto.IsPublic)
        {
            query = query.Where(d =>
                d.Status != null &&
                d.Status.Name.Trim().ToLower() == "publicado");
        }

        if (searchSortRequestDto.IsPublic && searchSortRequestDto.FilterBy?.Trim().ToLower() == "status")
        {
            searchSortRequestDto.FilterBy = null;
        }

        // Search
        if (!string.IsNullOrEmpty(searchSortRequestDto.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDto.Search))
        {
            string search = searchSortRequestDto.Search.ToLower();

            query = searchSortRequestDto.FilterBy?.ToLower() switch
            {
                "status" => query.Where(d =>
                    d.Status != null &&
                    EF.Functions.ILike(d.Status.Name, $"%{search}%")),

                "type" => query.Where(d =>
                    d.DocumentType != null &&
                    EF.Functions.ILike(d.DocumentType.Name, $"%{search}%")),

                "category" => query.Where(d =>
                    d.Categories.Any(c =>
                        EF.Functions.ILike(c.Name, $"%{search}%"))),

                _ => query.Where(d =>
                    EF.Functions.ILike(d.Title, $"%{search}%")),
            };
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(d => d.Title),
            ("title", _) => query.OrderByDescending(d => d.Title),

            ("date", "asc") => query.OrderBy(d => d.PublicationDate),
            _ => query.OrderByDescending(d => d.PublicationDate)
        };

        return new PageResponseDto<DocumentMinimalResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(DocumentExtensions.MapDocumentMinimalResponseDto())
                .ToListAsync(),
        };
    }

    public async Task<Document> GetDocument(Guid id)
    {
        var document = await context.Documents
            .AsNoTracking()
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .Include(d => d.DocumentType)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document is null)
            throw new NotFoundException("El documento seleccionada no existe.");

        return document;
    }

    public async Task<Document> GetDocumentBySlug(string slug)
    {
        var document = await context.Documents
            .AsNoTracking()
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .Include(d => d.DocumentType)
            .FirstOrDefaultAsync(d => d.Slug == slug);

        if(document?.Status != null && document.Status.Name.Trim().ToLower() == "publicado")
            return document;

        throw new NotFoundException("El documento seleccionada no existe.");
    }
}