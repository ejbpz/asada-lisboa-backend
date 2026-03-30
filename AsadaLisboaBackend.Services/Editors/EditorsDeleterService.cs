using HtmlAgilityPack;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors
{
    public class EditorsDeleterService : IEditorsDeleterService
    {
        private readonly IFileSystemsManager _fileSystems;

        public EditorsDeleterService(IFileSystemsManager fileSystems)
        {
            _fileSystems = fileSystems;
        }

        public async Task DeletePrincipalImage(string fileName)
        {
            await _fileSystems.DeleteAsync(fileName, "news");
        }

        public async Task DeleteContentImages(string html)
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
                await _fileSystems.DeleteAsync(fileName, "news");
            }
        }

        public async Task DeleteUnusedImages(string oldHtml, string newHtml)
        {
            var oldDoc = new HtmlDocument();
            oldDoc.LoadHtml(oldHtml);

            var oldNodes = oldDoc.DocumentNode
                .SelectNodes("//img[contains(@src, '/news/')]");

            var newDoc = new HtmlDocument();
            newDoc.LoadHtml(newHtml);

            var newNodes = newDoc.DocumentNode
                .SelectNodes("//img[contains(@src, '/news/')]");

            if (oldNodes is null)
                return;

            var oldFileNames = oldNodes
                .Select(node => Path.GetFileName(node.GetAttributeValue("src", null!)))
                .Where(fileName => !string.IsNullOrEmpty(fileName))
                .ToList();

            var newFileNames = newNodes
                .Select(node => Path.GetFileName(node.GetAttributeValue("src", null!)))
                .Where(fileName => !string.IsNullOrEmpty(fileName))
                .ToList();

            var notRelatedFileNames = oldFileNames.Except(newFileNames).ToList();

            foreach (var fileName in notRelatedFileNames)
            {
                await _fileSystems.DeleteAsync(fileName, "news");
            }
        }
    }
}
