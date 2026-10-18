using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges;

public interface IChargesAdderRepository
{
    public Task<ChargeResponseDto> CreateCharge(Charge charge);
}