using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Repositories.AboutUsSections;

public class AboutUsSectionsGetterRepository(ApplicationDbContext context) : IAboutUsSectionsGetterRepository
{
    public async Task<PageResponseDto<AboutUsResponseDto>> GetAboutUsSections(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<AboutUsSection> query = context.AboutUsSections
            .AsNoTracking();

        // Search
        if (!string.IsNullOrEmpty(searchSortRequestDto.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDto.Search))
        {
            string search = searchSortRequestDto.Search.ToLower();

            query = query.Where(a =>
                EF.Functions.ILike(a.SectionType, $"%{search}%"));
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(u => u.SectionType),
            ("name", _) => query.OrderBy(u => u.SectionType),
            (_, "desc") => query.OrderByDescending(u => (u.Order)),
            _ => query.OrderBy(u => (u.Order)),
        };

        return new PageResponseDto<AboutUsResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(AboutUsExtensions.MapAboutUsResponseDto())
                .ToListAsync(),
        };
    }
}