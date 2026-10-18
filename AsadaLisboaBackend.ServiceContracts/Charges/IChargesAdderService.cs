using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.ServiceContracts.Charges;

public interface IChargesAdderService
{
    public Task<ChargeResponseDto> CreateCharge(string nameCharge);
}