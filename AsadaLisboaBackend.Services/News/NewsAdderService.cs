using Microsoft.EntityFrameworkCore;
using HtmlAgilityPack;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemManager _fileSystem;
        private readonly INewsAdderRepository _newsAdderRepository;

        public NewsAdderService(INewsAdderRepository newsAdderRepository, ApplicationDbContext context, IFileSystemManager fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
            _newsAdderRepository = newsAdderRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO)
        {
            var id = Guid.NewGuid();

            var imageUrl = await _fileSystem.SaveAsync(newRequestDTO.File, "news");
            var fileName = Path.GetFileName(imageUrl);

            var content = ChangeImages(newRequestDTO.Description);

            var categories = await _context.Categories
                .Where(c => newRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();

            var newModel = new New()
            {
                Id = id,
                FileName = fileName,
                ImageUrl = imageUrl,
                Categories = categories,
                Title = newRequestDTO.Title,
                FilePath = $"news/{fileName}",
                StatusId = newRequestDTO.StatusId,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
                Slug = GenerateSlug.New(newRequestDTO.Title, id),
                Description = newRequestDTO.Description.ToString(),
            };

            var created = await _newsAdderRepository.CreateNew(newModel);

            if (created is null)
                throw new CreateObjectException("Error al crear la noticia.");

            return created.ToNewResponseDTO();
        }

        private async Task<string> ChangeImages(string html)
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

                await _fileSystem.MoveAsync(fileName, "temp", "news");

                node.SetAttributeValue("src", $"/news/{fileName}");
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
