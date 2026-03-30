namespace AsadaLisboaBackend.ServiceContracts.Editors
{
    public interface IEditorsDeleterService
    {
        public Task DeletePrincipalImage(string fileName);
        public Task DeleteContentImages(string html);
        public Task DeleteUnusedImages(string oldHtml, string newHtml);
    }
}
