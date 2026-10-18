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

public class NewsAdderService(
    INewsAdderRepository newsAdderRepository,
    IEditorsUpdaterService editorsUpdaterService,
    IStatusesGetterRepository statusesGetterRepository,
    ICategoriesGetterService categoriesGetterService,
    IFileSystemsManager fileSystems,
    ILogger<NewsAdderService> logger,
    IMemoryCachesService memoryCachesService)
    : INewsAdderService
{
    public async Task<NewResponseDto> CreateNew(NewRequestDto newRequestDto)
    {
        var id = Guid.NewGuid();

        var imageUrl = string.Empty;
        var fileName = string.Empty;
        var filePath = string.Empty;

        var slug = GenerateSlug.New(newRequestDto.Title, id);

        if (newRequestDto.File is not null && newRequestDto.File.Length > 0)
        {
            imageUrl = await fileSystems.SaveAsync(newRequestDto.File, "noticias", slug);
            fileName = Path.GetFileName(imageUrl);
        }

        if(!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrWhiteSpace(imageUrl) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrWhiteSpace(fileName))
            filePath = $"noticias/{fileName}";

        var sanitizer = HtmlSanitizerFactory.Create();
        var cleanHtml = sanitizer.Sanitize(newRequestDto.Description);
        var content = await editorsUpdaterService.ChangeHtmlImagesFolder(cleanHtml);

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
            Slug = slug,
            FilePath = filePath,
            FileName = fileName,
            ImageUrl = imageUrl,
            Description = content,
            Categories = categories,
            Title = newRequestDto.Title,
            PublicationDate = DateTime.UtcNow,
            LastEditionDate = DateTime.UtcNow,
        };

        var created = await newsAdderRepository.CreateNew(newModel);

        logger.LogInformation("Noticia con id {Id} creada exitosamente.", created.Id);

        memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

        return created.ToNewResponseDto();
    }
}