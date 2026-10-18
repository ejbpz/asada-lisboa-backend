using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images;

public class ImagesGetterRepository(ApplicationDbContext context) : IImagesGetterRepository
{
    public async Task<PageResponseDto<ImageMinimalResponseDto>> GetImages(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<Image> query = context.Images
            .AsNoTracking()
            .Include(i => i.Status)
            .Include(i => i.Categories);

        if (searchSortRequestDto.IsPublic)
        {
            query = query.Where(i =>
                i.Status != null &&
                i.Status.Name.Trim().ToLower() == "publicado");
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
                "status" => query.Where(i =>
                    i.Status != null &&
                    EF.Functions.ILike(i.Status.Name, $"%{search}%")),

                "category" => query.Where(i =>
                    i.Categories.Any(c =>
                        EF.Functions.ILike(c.Name, $"%{search}%"))),

                _ => query.Where(i =>
                    EF.Functions.ILike(i.Title, $"%{search}%")),
            };
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(i => i.Title),
            ("title", _) => query.OrderByDescending(i => i.Title),

            ("date", "asc") => query.OrderBy(i => i.PublicationDate),
            _ => query.OrderByDescending(i => i.PublicationDate)
        };

        return new PageResponseDto<ImageMinimalResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(ImageExtensions.MapImageMinimalResponseDto())
                .ToListAsync(),
        };
    }

    public async Task<Image> GetImage(Guid id)
    {
        var image = await context.Images
            .AsNoTracking()
            .Include(i => i.Status)
            .Include(i => i.Categories)
            .FirstOrDefaultAsync(i => i.Id == id);

        return image ?? throw new NotFoundException("La imagen seleccionada no existe.");
    }

    public async Task<Image> GetImageBySlug(string slug)
    {
        var image = await context.Images
            .AsNoTracking()
            .Include(i => i.Status)
            .Include(i => i.Categories)
            .FirstOrDefaultAsync(i => i.Slug == slug);

        if (image?.Status != null && image.Status.Name.Trim().ToLower() == "publicado")
            return image;

        throw new NotFoundException("La imagen seleccionada no existe.");
    }
}