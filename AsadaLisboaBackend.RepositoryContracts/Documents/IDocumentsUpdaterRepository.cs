
namespace AsadaLisboaBackend.RepositoryContracts.Documents
{
    public interface IDocumentsUpdaterRepository
    {
        public Task<Models.Document> UpdateDocument(Models.Document document);
    }
}
