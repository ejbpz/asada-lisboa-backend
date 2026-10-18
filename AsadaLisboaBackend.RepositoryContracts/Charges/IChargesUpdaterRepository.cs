using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges;

public interface IChargesUpdaterRepository
{
    public Task<ChargeResponseDto> UpdateCharge(Guid id, string chargeRequest);
}