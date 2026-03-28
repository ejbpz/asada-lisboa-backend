using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Repositories.AboutUsSections
{
    public class AboutUsSectionsGetterRepository : IAboutUsSectionsGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public AboutUsSectionsGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<AboutUsResponseDTO>> GetAboutUsSections(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<AboutUsSection> query = _context.AboutUsSections
                .AsNoTracking();

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = query.Where(a =>
                        EF.Functions.Like(a.SectionType, $"%{search}%"));
            }

            // Sort
            query = (searchSortRequestDTO.SortBy.ToLower(), searchSortRequestDTO.SortDirection.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(u => u.SectionType),
                ("name", _) => query.OrderBy(u => u.SectionType),
                (_, "desc") => query.OrderByDescending(u => (u.Order)),
                _ => query.OrderBy(u => (u.Order)),
            };

            return new PageResponseDTO<AboutUsResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(AboutUsExtensions.MapAboutUsResponseDTO())
                    .ToListAsync(),
            };
        }
    }
}
