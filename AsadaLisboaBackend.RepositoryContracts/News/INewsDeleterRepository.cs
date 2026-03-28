namespace AsadaLisboaBackend.RepositoryContracts.News
{
    public interface INewsDeleterRepository
    {
        public Task DeleteNew(Guid id);
    }
}
