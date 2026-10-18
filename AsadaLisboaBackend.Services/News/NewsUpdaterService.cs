using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.HtmlSanitizer;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News;

public class NewsUpdaterService(
    INewsUpdaterRepository newsUpdaterRepository,
    INewsGetterRepository newsGetterRepository,
    IEditorsUpdaterService editorsUpdaterService,
    IEditorsDeleterService editorsDeleterService,
    IStatusesGetterRepository statusesGetterRepository,
    ICategoriesGetterService categoriesGetterService,
    IFileSystemsManager fileSystems,
    ILogger<NewsUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : INewsUpdaterService
{
    public async Task<NewResponseDto> UpdateNew(Guid id, NewRequestDto newRequestDto)
    {
        var existingNew = await newsGetterRepository.GetNew(id);

        var imageUrl = existingNew.ImageUrl;
        var fileName = existingNew.FileName;
        var filePath = existingNew.FilePath;

        existingNew.Slug = GenerateSlug.New(newRequestDto.Title, id);

        if (newRequestDto.File is not null)
        {
            if (!string.IsNullOrEmpty(existingNew.FilePath) && File.Exists(existingNew.FilePath))
                File.Delete(existingNew.FilePath);

            var newImageUrl = await fileSystems.SaveAsync(newRequestDto.File, "noticias", existingNew.Slug);

            imageUrl = newImageUrl;
            fileName = Path.GetFileName(imageUrl);
            filePath = $"noticias/{fileName}";
        }

        var sanitizer = HtmlSanitizerFactory.Create();
        var cleanHtml = sanitizer.Sanitize(newRequestDto.Description);

        var content = await editorsUpdaterService.ChangeHtmlImagesFolder(cleanHtml);
        await editorsDeleterService.DeleteUnusedImages(existingNew.Description, newRequestDto.Description);

        if (newRequestDto.StatusId == Guid.Empty)
        {
            logger.LogError("StatusId no puede ser vacío.");
            throw new ArgumentException("StatusId inválido.");
        }

        var status = await statusesGetterRepository.GetStatus(newRequestDto.StatusId);
        var categories = await categoriesGetterService.ToCreateCategories(newRequestDto.Categories);


        var newModel = new New()
        {
            Id = id,
            StatusId = status.Id,
            ImageUrl = imageUrl,
            FileName = fileName,
            FilePath = filePath,
            Description = content,
            Categories = categories,
            Slug = existingNew.Slug,
            Title = newRequestDto.Title,
            LastEditionDate = DateTime.UtcNow,
            PublicationDate = existingNew.PublicationDate,
        };

        var updated = await newsUpdaterRepository.UpdateNew(id, newModel);

        logger.LogInformation("Noticia con id {Id} actualizada correctamente", updated.Id);

        memoryCachesService.RemoveById(Constants.CACHE_NEWS, updated.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

        return updated.ToNewResponseDto();
    }
}