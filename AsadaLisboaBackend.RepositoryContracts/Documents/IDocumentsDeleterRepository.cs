namespace AsadaLisboaBackend.RepositoryContracts.Documents
{
    public interface IDocumentsDeleterRepository
    {
        public Task DeleteDocument(Guid id);
    }
}
