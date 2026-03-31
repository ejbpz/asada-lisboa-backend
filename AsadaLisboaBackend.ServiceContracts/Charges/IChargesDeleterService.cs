namespace AsadaLisboaBackend.ServiceContracts.Charges
{
    public interface IChargesDeleterService
    {
        public Task DeleteCharge(Guid id);
    }
}
