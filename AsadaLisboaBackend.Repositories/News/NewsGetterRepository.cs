using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News
{
    public class NewsGetterRepository : INewsGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NewResponseDTO> GetNew(Guid id)
        {
            var newModel = await _context.News
                .AsNoTracking()
                .Include(n => n.Status)
                .Include(n => n.Categories)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (newModel is null)
                throw new NotFoundException("La noticia seleccionada no existe.");

            return newModel.ToNewResponseDTO();
        }

        public async Task<NewResponseDTO> GetNewBySlug(string slug)
        {
            var newModel = await _context.News
                .AsNoTracking()
                .Include(n => n.Status)
                .Include(n => n.Categories)
                .FirstOrDefaultAsync(n => n.Slug == slug);

            if (newModel is null)
                throw new NotFoundException("La noticia seleccionada no existe.");

            return newModel.ToNewResponseDTO();
        }

        public async Task<PageResponseDTO<NewMinimalResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<New> query = _context.News
                .AsNoTracking()
                .Include(n => n.Status)
                .Include(n => n.Categories);

            if (searchSortRequestDTO.IsPublic)
            {
                query = query.Where(n =>
                    n.Status != null &&
                    n.Status.Name.Trim().ToLower() == "publicado");
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
                    "status" => query.Where(n =>
                        n.Status != null &&
                        EF.Functions.Like(n.Status.Name, $"%{search}%")),

                    "category" => query.Where(n =>
                        n.Categories.Any(c =>
                            EF.Functions.Like(c.Name, $"%{search}%"))),

                    _ => query.Where(n =>
                        EF.Functions.Like(n.Title, $"%{search}%")),
                };
            }

            // Sort
            query = (searchSortRequestDTO.SortBy?.ToLower(), searchSortRequestDTO.SortDirection?.ToLower()) switch
            {
                ("date", "desc") => query.OrderByDescending(n => n.PublicationDate),
                ("date", _) => query.OrderBy(n => n.PublicationDate),

                ("title", "desc") => query.OrderByDescending(n => n.Title),
                ("title", _) => query.OrderBy(n => n.Title),

                _ => query.OrderByDescending(n => n.PublicationDate)
            };

            return new PageResponseDTO<NewMinimalResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(NewExtensions.MapNewMinimalResponseDTO())
                    .ToListAsync(),
            };
        }


        public async Task<List<NewMinimalResponseDTO>> GetRecommendedNews(string slug)
        {
            var newData = await GetNewBySlug(slug);

            if (newData is null)
                throw new NotFoundException("Error al obtener noticia.");

            var take = 8;

            var categoriesIds = await _context.News
                .Where(n => n.Id == newData.Id)
                .SelectMany(n => n.Categories.Select(nc => nc.Id))
                .ToListAsync();

            var relatedNews = new List<NewMinimalResponseDTO>();

            if (categoriesIds.Any())
            {
                relatedNews = await _context.News
                    .Where(n => n.Id != newData.Id)
                    .Select(n => new
                    {
                        News = n,
                        MatchCount = n.Categories
                            .Count(nc => categoriesIds.Contains(nc.Id))
                    })
                    .Where(x => x.MatchCount > 0)
                    .OrderByDescending(x => x.MatchCount)
                    .ThenByDescending(x => x.News.LastEditionDate)
                    .Take(take)
                    .Select(x => x.News)
                    .Select(NewExtensions.MapNewMinimalResponseDTO())
                    .ToListAsync();
            }

            if (relatedNews.Count < take)
            {
                var missing = take - relatedNews.Count;

                var searchShortRequestDTO = new SearchSortRequestDTO()
                {
                    IsPublic = true,
                    Take = take * 2,
                    Offset = 0,
                };

                var fallbackRaw = (await GetNews(searchShortRequestDTO)).Data;
                var existingIds = new HashSet<Guid>(relatedNews.Select(n => n.Id));

                existingIds.Add(newData.Id);

                var fallbackFiltered = fallbackRaw
                    .Where(n => !existingIds.Contains(n.Id))
                    .Take(missing)
                    .ToList();

                relatedNews.AddRange(fallbackFiltered);
            }

            return relatedNews;
        }
    }
}
