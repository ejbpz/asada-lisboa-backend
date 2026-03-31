using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges
{
    public interface IChargesUpdaterRepository
    {
        public Task<ChargeResponseDTO> UpdateCharge(Guid id, string chargeRequest);
    }
}
