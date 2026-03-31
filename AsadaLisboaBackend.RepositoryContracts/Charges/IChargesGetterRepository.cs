using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges
{
    public interface IChargesGetterRepository
    {
        public Task<bool> ExistsCharge(string name);
        public Task<List<ChargeResponseDTO>> GetCharges();
        public Task<ChargeResponseDTO> GetCharge(Guid id);
    }
}
