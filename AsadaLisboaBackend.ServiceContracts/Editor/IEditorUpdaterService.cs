namespace AsadaLisboaBackend.ServiceContracts.Editor
{
    public interface IEditorUpdaterService
    {
        public Task<string> ChangeHtmlImagesFolder(string html);
    }
}
