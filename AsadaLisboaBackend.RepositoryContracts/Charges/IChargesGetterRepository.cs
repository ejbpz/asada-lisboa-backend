using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges
{
    public interface IChargesGetterRepository
    {
        public Task<List<ChargeResponseDTO>> GetCharges();
        public Task<ChargeResponseDTO> GetCharge(Guid id);
    }
}
