using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesGetterService : IChargesGetterService
    {
        private readonly IChargesGetterRepository _chargesGetterRepository;

        public ChargesGetterService(IChargesGetterRepository chargesGetterRepository)
        {
            _chargesGetterRepository = chargesGetterRepository;
        }

        public async Task<List<ChargeResponseDTO>> GetCharges()
        {
            return await _chargesGetterRepository.GetCharges();
        }

        public async Task<bool> ExistsCharge(string name)
        {
            name = name.Trim().ToLower();
            return await _chargesGetterRepository.ExistsCharge(name);
        }
    }
}
