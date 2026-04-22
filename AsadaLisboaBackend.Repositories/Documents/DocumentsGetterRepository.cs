using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;


namespace AsadaLisboaBackend.Repositories.Documents
{
    public class DocumentsGetterRepository : IDocumentsGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentsGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<DocumentMinimalResponseDTO>> GetDocuments(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<Models.Document> query = _context.Documents
                   .AsNoTracking()
                   .Include(d => d.Status)
                   .Include(d => d.Categories)
                   .Include(d => d.DocumentType);

            if (searchSortRequestDTO.IsPublic)
            {
                query = query.Where(d =>
                    d.Status != null &&
                    d.Status.Name.Trim().ToLower() == "publicado");
            }

            if (searchSortRequestDTO.IsPublic && searchSortRequestDTO.FilterBy?.Trim().ToLower() == "status")
            {
                searchSortRequestDTO.FilterBy = null;
            }

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = searchSortRequestDTO.FilterBy?.ToLower() switch
                {
                    "status" => query.Where(d =>
                        d.Status != null &&
                        EF.Functions.Like(d.Status.Name, $"%{search}%")),

                    "type" => query.Where(d =>
                    d.DocumentType != null &&
                    EF.Functions.Like(d.DocumentType.Name, $"%{search}%")),

                    "category" => query.Where(d =>
                        d.Categories.Any(c =>
                            EF.Functions.Like(c.Name, $"%{search}%"))),

                    _ => query.Where(d =>
                        EF.Functions.Like(d.Title, $"%{search}%")),
                };
            }

            // Sort
            query = (searchSortRequestDTO.SortBy?.ToLower(), searchSortRequestDTO.SortDirection?.ToLower()) switch
            {
                ("date", "desc") => query.OrderByDescending(d => d.PublicationDate),
                ("date", _) => query.OrderBy(d => d.PublicationDate),

                ("title", "desc") => query.OrderByDescending(d => d.Title),
                ("title", _) => query.OrderBy(d => d.Title),

                _ => query.OrderByDescending(d => d.PublicationDate)
            };

            return new PageResponseDTO<DocumentMinimalResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(DocumentExtensions.MapDocumentMinimalResponseDTO())
                    .ToListAsync(),
            };
        }

        public async Task<Document> GetDocument(Guid id)
        {
            var document = await _context.Documents
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
            var document = await _context.Documents
                    .AsNoTracking()
                    .Include(d => d.Status)
                    .Include(d => d.Categories)
                    .Include(d => d.DocumentType)
                    .FirstOrDefaultAsync(d => d.Slug == slug);

            if (document is null)
                throw new NotFoundException("El documento seleccionada no existe.");

            return document;
        }
    }
}
