using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors;

public class EditorsUpdaterService(
    IFileSystemsManager fileSystems, 
    ILogger<EditorsUpdaterService> logger)
    : IEditorsUpdaterService
{
    public async Task<string> ChangeHtmlImagesFolder(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var nodeCollection = doc.DocumentNode
            .SelectNodes("//img[contains(@src, '/temp/')]");

        foreach (var node in nodeCollection)
        {
            var src = node.GetAttributeValue("src", null!);

            if (string.IsNullOrEmpty(src) || string.IsNullOrWhiteSpace(src))
                continue;

            var fileName = Path.GetFileName(src);

            await fileSystems.MoveAsync(fileName, "temp", "noticias");

            node.SetAttributeValue("src", $"{Constants.DOMAIN_HOST}/noticias/{fileName}");
        }

        logger.LogInformation("Cambiando imágenes de carpeta 'temp' a 'noticias'");
        return doc.DocumentNode.OuterHtml;
    }
}