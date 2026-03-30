using HtmlAgilityPack;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Editors
{
    public class EditorsUpdaterService : IEditorsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;

        public EditorsUpdaterService(IFileSystemsManager fileSystems)
        {
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

                if (src is null || string.IsNullOrEmpty(src))
                    continue;

                var fileName = Path.GetFileName(src);

                await _fileSystems.MoveAsync(fileName, "temp", "news");

                node.SetAttributeValue("src", $"/news/{fileName}");
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
