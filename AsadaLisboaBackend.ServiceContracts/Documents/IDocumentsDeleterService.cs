namespace AsadaLisboaBackend.ServiceContracts.Documents
{
    public interface IDocumentsDeleterService
    {
        public Task DeleterDocument(Guid id);
    }
}
