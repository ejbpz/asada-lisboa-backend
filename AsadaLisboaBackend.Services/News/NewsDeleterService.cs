using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.FileSystem;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Services.Exceptions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsDeleterService : INewsDeleterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileSystemManager _fileSystem;
        private readonly INewsDeleterRepository _newsDeleterRepository;

        public NewsDeleterService(INewsDeleterRepository newsDeleterRepository, IFileSystemManager fileSystem, ApplicationDbContext context)
        {
            _context = context;
            _fileSystem = fileSystem;
            _newsDeleterRepository = newsDeleterRepository;
        }

        public async Task DeleteNew(Guid id)
        {
            var existingNew = await _context.News
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (existingNew is null)
                throw new NotFoundException("La noticia no fue encontrada.");

            await DeletePrincipalImage(existingNew.FileName);

            await DeleteContentImages(existingNew.Description);

            await _newsDeleterRepository.DeleteNew(id);
        }

        private async Task DeletePrincipalImage(string fileName)
        {
            await _fileSystem.DeleteAsync(fileName, "news");
        }

        private async Task DeleteContentImages(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodeCollection = doc.DocumentNode
                .SelectNodes("//img[contains(@src, '/news/')]");

            if (nodeCollection is null)
                return;

            var fileNames = nodeCollection
                .Select(node => node.GetAttributeValue("src", null!))
                .Where(src => !string.IsNullOrEmpty(src))
                .Select(src => Path.GetFileName(src!))
                .ToList();

            foreach (var fileName in fileNames)
            {
                await _fileSystem.DeleteAsync(fileName, "news");
            }
        }
    }
}
