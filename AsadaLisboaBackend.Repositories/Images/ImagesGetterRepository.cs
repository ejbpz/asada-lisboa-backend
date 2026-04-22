using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images
{
    public class ImagesGetterRepository : IImagesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public ImagesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<ImageMinimalResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<Image> query = _context.Images
                .AsNoTracking()
                .Include(i => i.Status)
                .Include(i => i.Categories);

            if (searchSortRequestDTO.IsPublic)
            {
                query = query.Where(i =>
                    i.Status != null &&
                    i.Status.Name.Trim().ToLower() == "publicado");
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
                    "status" => query.Where(i =>
                        i.Status != null &&
                        EF.Functions.Like(i.Status.Name, $"%{search}%")),

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

            return new PageResponseDTO<ImageMinimalResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(ImageExtensions.MapImageMinimalResponseDTO())
                    .ToListAsync(),
            };
        }

        public async Task<Image> GetImage(Guid id)
        {
            var image = await _context.Images
                .AsNoTracking()
                .Include(i => i.Status)
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image is null)
                throw new NotFoundException("La imagen seleccionada no existe.");

            return image;
        }

        public async Task<Image> GetImageBySlug(string slug)
        {
            var image = await _context.Images
                .AsNoTracking()
                .Include(i => i.Status)
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Slug == slug);

            if (image is null)
                throw new NotFoundException("La imagen seleccionada no existe.");

            return image;
        }
    }
}
