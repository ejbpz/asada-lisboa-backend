using HtmlAgilityPack;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Editor
{
    public class EditorUpdaterService : IEditorUpdaterService
    {
        private readonly IFileSystemManager _fileSystem;

        public EditorUpdaterService(IFileSystemManager fileSystem)
        {
            _fileSystem = fileSystem;
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

                await _fileSystem.MoveAsync(fileName, "temp", "news");

                node.SetAttributeValue("src", $"/news/{fileName}");
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
