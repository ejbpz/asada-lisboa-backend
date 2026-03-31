using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesUpdaterService : IChargesUpdaterService
    {
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesUpdaterRepository _chargesUpdaterRepository;

        public ChargesUpdaterService(IChargesGetterService chargesGetterService, IChargesUpdaterRepository chargesUpdaterRepository)
        {
            _chargesGetterService = chargesGetterService;
            _chargesUpdaterRepository = chargesUpdaterRepository;
        }

        public async Task<ChargeResponseDTO> UpdateCharge(Guid id, string chargeRequest)
        {
            var existsCharge = await _chargesGetterService.ExistsCharge(chargeRequest);

            if(existsCharge)
                throw new Exception("El nombre del cargo ya existe."); // TODO

            return await _chargesUpdaterRepository.UpdateCharge(id, chargeRequest);
        }
    }
}
