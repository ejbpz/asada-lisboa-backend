using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Repositories.Configurations
{
    public class ConfigurationsGetterRepository : IConfigurationsGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationsGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<ConfigurationResponseDTO>> GetConfigurations(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<VisualSetting> query = _context.VisualSettings
                .AsNoTracking();

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = query.Where(s =>
                        EF.Functions.Like(s.SettingType, $"%{search}%"));
            }

            // Sort
            query = (searchSortRequestDTO.SortBy.ToLower(), searchSortRequestDTO.SortDirection.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(u => u.SettingType),
                ("name", _) => query.OrderBy(u => u.SettingType),
                (_, "desc") => query.OrderByDescending(u => (u.Order)),
                _ => query.OrderBy(u => (u.Order)),
            };

            return new PageResponseDTO<ConfigurationResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(ConfigurationExtensions.MapConfigurationResponseDTO())
                    .ToListAsync(),
            };
        }
    }
}
