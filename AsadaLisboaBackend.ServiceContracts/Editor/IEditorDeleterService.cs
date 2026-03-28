namespace AsadaLisboaBackend.ServiceContracts.Editor
{
    public interface IEditorDeleterService
    {
        public Task DeletePrincipalImage(string fileName);
        public Task DeleteContentImages(string html);
        public Task DeleteUnusedImages(string oldHtml, string newHtml);
    }
}
