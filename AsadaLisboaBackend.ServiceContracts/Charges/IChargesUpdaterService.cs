using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.ServiceContracts.Charges;

public interface IChargesUpdaterService
{
    public Task<ChargeResponseDto> UpdateCharge(Guid id, string chargeRequest);
}