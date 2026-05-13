using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Search;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Searches;

namespace AsadaLisboaBackend.Services.Searches
{
    public class SearchesGetterService : ISearchesGetterService
    {
        private readonly ApplicationDbContext _context;

        public SearchesGetterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SearchResponseDTO>> Search(string query)
        {
            query = query.Trim();

            if (query.Trim().Length < 2)
            {
                return new List<SearchResponseDTO>();
            }

            var news = await _context.News
                .AsNoTracking()
                .Include(x => x.Status)
                .Where(x =>
                    x.Status != null && x.Status.Name == "Publicado" &&
                    (
                        EF.Functions.ILike(x.Title, $"%{query}%") ||
                        EF.Functions.ILike(x.Description, $"%{query}%")
                    )
                )
                .OrderByDescending(x => x.PublicationDate)
                .Take(6)
                .Select(x => new SearchResponseDTO
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    Title = x.Title,
                    Type = "Noticia",
                    Description = x.Description,
                    PublicationDate = x.PublicationDate
                })
                .ToListAsync();

            var documents = await _context.Documents
                .AsNoTracking()
                .Include(x => x.Status)
                .Where(x =>
                    x.Status != null && x.Status.Name == "Publicado" &&
                    (
                        EF.Functions.ILike(x.Title, $"%{query}%") ||
                        EF.Functions.ILike(x.Description, $"%{query}%")
                    )
                )
                .OrderByDescending(x => x.PublicationDate)
                .Take(6)
                .Select(x => new SearchResponseDTO
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    Title = x.Title,
                    Type = "Documento",
                    Description = x.Description,
                    PublicationDate = x.PublicationDate
                })
                .ToListAsync();

            var images = await _context.Images
                .AsNoTracking()
                .Include(x => x.Status)
                .Where(x =>
                    x.Status != null && x.Status.Name == "Publicado" &&
                    (
                        EF.Functions.ILike(x.Title, $"%{query}%") ||
                        EF.Functions.ILike(x.Description, $"%{query}%")
                    )
                )
                .OrderByDescending(x => x.PublicationDate)
                .Take(6)
                .Select(x => new SearchResponseDTO
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    Type = "Imagen",
                    Title = x.Title,
                    Description = x.Description,
                    PublicationDate = x.PublicationDate
                })
                .ToListAsync();

            return [.. news,.. documents,.. images];
        }
    }
}
