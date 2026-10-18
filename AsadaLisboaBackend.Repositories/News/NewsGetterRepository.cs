using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News;

public class NewsGetterRepository(ApplicationDbContext context) : INewsGetterRepository
{
    public async Task<NewResponseDto> GetNew(Guid id)
    {
        var newModel = await context.News
            .AsNoTracking()
            .Include(n => n.Status)
            .Include(n => n.Categories)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (newModel is null)
            throw new NotFoundException("La noticia seleccionada no existe.");

        return newModel.ToNewResponseDto();
    }

    public async Task<NewResponseDto> GetNewBySlug(string slug)
    {
        var newModel = await context.News
            .AsNoTracking()
            .Include(n => n.Status)
            .Include(n => n.Categories)
            .FirstOrDefaultAsync(n => n.Slug == slug);

        if (newModel?.Status != null && newModel.Status.Name.Trim().ToLower() == "publicado")
            return newModel.ToNewResponseDto();

        throw new NotFoundException("La noticia seleccionada no existe.");
    }

    public async Task<PageResponseDto<NewMinimalResponseDto>> GetNews(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<New> query = context.News
            .AsNoTracking()
            .Include(n => n.Status)
            .Include(n => n.Categories);

        if (searchSortRequestDto.IsPublic)
        {
            query = query.Where(n =>
                n.Status != null &&
                n.Status.Name.Trim().ToLower() == "publicado");
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
                "status" => query.Where(n =>
                    n.Status != null &&
                    EF.Functions.ILike(n.Status.Name, $"%{search}%")),

                "category" => query.Where(n =>
                    n.Categories.Any(c =>
                        EF.Functions.ILike(c.Name, $"%{search}%"))),

                _ => query.Where(n =>
                    EF.Functions.ILike(n.Title, $"%{search}%")),
            };
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(n => n.Title),
            ("title", _) => query.OrderByDescending(n => n.Title),

            ("date", "asc") => query.OrderBy(n => n.LastEditionDate),
            _ => query.OrderByDescending(n => n.LastEditionDate)
        };

        return new PageResponseDto<NewMinimalResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(NewExtensions.MapNewMinimalResponseDto())
                .ToListAsync(),
        };
    }


    public async Task<List<NewMinimalResponseDto>> GetRecommendedNews(string slug)
    {
        var newData = await GetNewBySlug(slug);

        if (newData is null)
            throw new NotFoundException("Error al obtener noticia.");

        var take = Constants.PAGINATION_SIZE;

        var categoriesIds = await context.News
            .Where(n => n.Id == newData.Id)
            .SelectMany(n => n.Categories.Select(nc => nc.Id))
            .ToListAsync();

        var relatedNews = new List<NewMinimalResponseDto>();

        if (categoriesIds.Any())
        {
            relatedNews = await context.News
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
                .Select(NewExtensions.MapNewMinimalResponseDto())
                .ToListAsync();
        }

        if (relatedNews.Count < take)
        {
            var missing = take - relatedNews.Count;

            var searchShortRequestDto = new SearchSortRequestDto()
            {
                IsPublic = true,
                Take = take * 2,
                Offset = 0,
            };

            var fallbackRaw = (await GetNews(searchShortRequestDto)).Data;
            var existingIds = new HashSet<Guid>(relatedNews.Select(n => n.Id)) { newData.Id };

            var fallbackFiltered = fallbackRaw
                .Where(n => !existingIds.Contains(n.Id))
                .Take(missing)
                .ToList();

            relatedNews.AddRange(fallbackFiltered);
        }

        return relatedNews;
    }
}