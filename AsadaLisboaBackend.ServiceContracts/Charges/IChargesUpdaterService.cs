using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.ServiceContracts.Charges
{
    public interface IChargesUpdaterService
    {
        public Task<ChargeResponseDTO> UpdateCharge(Guid id, string chargeRequest);
    }
}
