using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors
{
    public class EditorsUpdaterService : IEditorsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<EditorsUpdaterService> _logger;

        public EditorsUpdaterService(IFileSystemsManager fileSystems, ILogger<EditorsUpdaterService> logger)
        {
            _logger = logger;
            _fileSystems = fileSystems;
        }

        public async Task<string> ChangeHtmlImagesFolder(string html)
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

                if (src is null || string.IsNullOrEmpty(src) || string.IsNullOrWhiteSpace(src))
                    continue;

                var fileName = Path.GetFileName(src);

                await _fileSystems.MoveAsync(fileName, "temp", "noticias");

                node.SetAttributeValue("src", $"{Constants.DOMAIN_HOST}/noticias/{fileName}");
            }

            _logger.LogInformation("Cambiando imágenes de carpeta 'temp' a 'noticias'");
            return doc.DocumentNode.OuterHtml;
        }
    }
}
