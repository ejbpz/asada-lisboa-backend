using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.ServiceContracts.Charges
{
    public interface IChargesGetterService
    {
        public Task<List<ChargeResponseDTO>> GetCharges();
    }
}
