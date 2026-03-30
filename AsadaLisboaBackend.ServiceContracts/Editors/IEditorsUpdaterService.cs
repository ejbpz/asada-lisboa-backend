namespace AsadaLisboaBackend.ServiceContracts.Editors
{
    public interface IEditorsUpdaterService
    {
        public Task<string> ChangeHtmlImagesFolder(string html);
    }
}
