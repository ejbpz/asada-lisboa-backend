namespace AsadaLisboaBackend.RepositoryContracts.Documents
{
    public interface IDocumentsAdderRepository
    {
        public Task<Models.Document> CreateDocument(Models.Document newDocument);
    }
}
