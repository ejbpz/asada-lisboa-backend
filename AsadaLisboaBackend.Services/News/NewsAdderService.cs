using Microsoft.EntityFrameworkCore;
using HtmlAgilityPack;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly IImageService _imageService;
        private readonly ApplicationDbContext _context;
        private readonly INewsAdderRepository _newsAdderRepository;
        private readonly INewsUpdaterRepository _newsUpdaterRepository;

        public NewsAdderService(IImageService imageService, INewsAdderRepository newsAdderRepository, INewsUpdaterRepository newsUpdaterRepository, ApplicationDbContext context)
        {
            _context = context;
            _imageService = imageService;
            _newsAdderRepository = newsAdderRepository;
            _newsUpdaterRepository = newsUpdaterRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO, FileChangeStorageOptions options)
        {
            var id = Guid.NewGuid();

            var principalImage = await CreateNewPrincipalImage(newRequestDTO, options);

            var slug = GenerateSlug.New(newRequestDTO.Title, id);
            var categories = await _context.Categories
                .Where(c => newRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();
            var filePath = principalImage.FilePath;
            var fileName = principalImage.FileName;
            var imageUrl = principalImage.Url;

            var newModel = new New()
            {
                Id = id,
                Slug = slug,
                FileName = fileName,
                FilePath = filePath,
                ImageUrl = imageUrl,
                Categories = categories,
                Title = newRequestDTO.Title,
                StatusId = newRequestDTO.StatusId,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
                Description = newRequestDTO.Description.ToString(),
            };

            var newCreated = await _newsAdderRepository.CreateNew(newModel);

            var content = ChangeImages(newRequestDTO.Description, options);
            newCreated.Description = content.ToString();

            return (await _newsUpdaterRepository.UpdateNew(id, newCreated))
                .ToNewResponseDTO();
        }

        private string ChangeImages(string html, FileChangeStorageOptions storageOptions)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodeCollection = doc.DocumentNode
                .SelectNodes("//img[contains(@src, '/temp/')]");

            if (nodeCollection is null)
                return html;

            foreach (var node in nodeCollection)
            {
                var src = node.GetAttributeValue("src", null!);

                if (src is null || string.IsNullOrEmpty(src))
                    continue;

                var fileName = Path.GetFileName(src);

                var sourcePath = Path.Combine(storageOptions.BasePath, fileName);
                var destinationPath = Path.Combine(storageOptions.NewBasePath, fileName);

                if (File.Exists(sourcePath))
                {
                    if (File.Exists(destinationPath))
                        File.Delete(destinationPath);

                    File.Move(sourcePath, destinationPath);
                }

                var newSrc = $"{storageOptions.NewBaseUrl}/{fileName}";
                node.SetAttributeValue("src", newSrc);
            }

            return doc.DocumentNode.OuterHtml;
        }

        private async Task<ImageResponseDTO> CreateNewPrincipalImage(NewRequestDTO newRequestDTO, FileChangeStorageOptions fileChangeStorageOptions)
        {
            var image = new ImageRequestDTO()
            {
                File = newRequestDTO.File,
                Title = newRequestDTO.Title,
                StatusId = newRequestDTO.StatusId,
                Description = newRequestDTO.Title,
            };

            var options = new FileStorageOptions()
            {
                BasePath = fileChangeStorageOptions.NewBasePath,
                BaseUrl = fileChangeStorageOptions.NewBaseUrl,
            };

            return await _imageService.CreateImage(image, options);
        }
    }
}
