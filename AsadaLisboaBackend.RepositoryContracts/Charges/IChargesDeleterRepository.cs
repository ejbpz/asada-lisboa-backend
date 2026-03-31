namespace AsadaLisboaBackend.RepositoryContracts.Charges
{
    public interface IChargesDeleterRepository
    {
        public Task DeleteCharge(Guid id);
    }
}
