namespace AsadaLisboaBackend.ServiceContracts.Document
{
    public interface IDocumentDeleterService
    {
        public Task DeleterDocument(Guid id);
    }
}
