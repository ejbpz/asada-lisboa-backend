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
                   .Include(i => i.Status)
                   .Include(i => i.Categories)
                   .Include(i => i.DocumentType);

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = searchSortRequestDTO.FilterBy?.ToLower() switch
                {
                    "status" => query.Where(i =>
                        i.Status != null &&
                        EF.Functions.Like(i.Status.Name, $"%{search}%")),

                    "type" => query.Where(i =>
                    i.DocumentType != null &&
                    EF.Functions.Like(i.DocumentType.Name, $"%{search}%")),

                    "category" => query.Where(i =>
                        i.Categories.Any(c =>
                            EF.Functions.Like(c.Name, $"%{search}%"))),

                    _ => query.Where(i =>
                        EF.Functions.Like(i.Title, $"%{search}%")),
                };
            }

            // Sort
            query = (searchSortRequestDTO.SortBy?.ToLower(), searchSortRequestDTO.SortDirection?.ToLower()) switch
            {
                ("date", "desc") => query.OrderByDescending(i => i.PublicationDate),
                ("date", _) => query.OrderBy(i => i.PublicationDate),

                ("title", "desc") => query.OrderByDescending(i => i.Title),
                ("title", _) => query.OrderBy(i => i.Title),

                _ => query.OrderByDescending(i => i.PublicationDate)
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
                    .FirstOrDefaultAsync(i => i.Id == id);

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
                    .FirstOrDefaultAsync(i => i.Slug == slug);

            if (document is null)
                throw new NotFoundException("El documento seleccionada no existe.");

            return document;
        }
    }
}
