using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.News
{
    public interface INewsAdderRepository
    {
        public Task<New> CreateNew(New newModel);
    }
}
