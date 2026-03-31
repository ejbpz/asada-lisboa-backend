using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.ServiceContracts.Charges
{
    public interface IChargesAdderService
    {
        public Task<ChargeResponseDTO> CreateCharge(string nameCharge);
    }
}
