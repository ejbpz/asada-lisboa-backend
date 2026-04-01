namespace AsadaLisboaBackend.RepositoryContracts.DocumentTypes
{
    public interface IDocumentTypesGetterRepository
    {
        public Guid? GetDocumentTypeIdByExtension(string extension);
    }
}
