using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors;

public class EditorsDeleterService(IFileSystemsManager fileSystems, ILogger<EditorsDeleterService> logger)
    : IEditorsDeleterService
{
    public async Task DeletePrincipalImage(string fileName)
    {
        await fileSystems.DeleteAsync(fileName, "noticias");
        logger.LogInformation($"Imagen principal '{fileName}' eliminada correctamente.");
    }

    public async Task DeleteContentImages(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var nodeCollection = doc.DocumentNode
            .SelectNodes("//img[contains(@src, '/noticias/')]");

        var fileNames = nodeCollection
            .Select(node => node.GetAttributeValue("src", null!))
            .Where(src => !string.IsNullOrEmpty(src))
            .Select(src => Path.GetFileName(src))
            .ToList();

        foreach (var fileName in fileNames)
        {
            await fileSystems.DeleteAsync(fileName, "noticias");
            logger.LogInformation($"Imagen de contenido '{fileName}' eliminada correctamente.");
        }
    }

    public async Task DeleteUnusedImages(string oldHtml, string newHtml)
    {
        var oldDoc = new HtmlDocument();
        oldDoc.LoadHtml(oldHtml);

        var oldNodes = oldDoc.DocumentNode
            .SelectNodes("//img[contains(@src, '/noticias/')]");

        var newDoc = new HtmlDocument();
        newDoc.LoadHtml(newHtml);

        var newNodes = newDoc.DocumentNode
            .SelectNodes("//img[contains(@src, '/noticias/')]");

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
            await fileSystems.DeleteAsync(fileName, "noticias");
            logger.LogInformation("Imagen no utilizada '{FileName}' eliminada correctamente.", fileName);
        }
    }
}