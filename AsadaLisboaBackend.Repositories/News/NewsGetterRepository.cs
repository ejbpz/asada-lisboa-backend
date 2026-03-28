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

        public async Task<PageResponseDTO<NewResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<New> query = _context.News
                .AsNoTracking()
                .Include(n => n.Status)
                .Include(n => n.Categories);

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

            return new PageResponseDTO<NewResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(NewExtensions.MapNewResponseDTO())
                    .ToListAsync(),
            };
        }
    }
}
