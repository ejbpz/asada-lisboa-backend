using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.News
{
    public interface INewsUpdaterRepository
    {
        public Task<New> UpdateNew(Guid id, New newModel);
    }
}
