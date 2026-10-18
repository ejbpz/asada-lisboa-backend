using AsadaLisboaBackend.Models.DTOs.Charge;

namespace AsadaLisboaBackend.RepositoryContracts.Charges;

public interface IChargesGetterRepository
{
    public Task<bool> ExistsCharge(string name);
    public Task<List<ChargeResponseDto>> GetCharges();
    public Task<ChargeResponseDto> GetCharge(Guid id);
}