using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesAdderService : IChargesAdderService
    {
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesAdderRepository _chargesAdderRepository;

        public ChargesAdderService(IChargesGetterService chargesGetterService, IChargesAdderRepository chargesAdderRepository)
        {
            _chargesGetterService = chargesGetterService;
            _chargesAdderRepository = chargesAdderRepository;
        }

        public async Task<ChargeResponseDTO> CreateCharge(string nameCharge)
        {
            var existsCharge = await _chargesGetterService.ExistsCharge(nameCharge);

            if (existsCharge)
                throw new Exception("El nombre del cargo ya existe.");

            var charge = new Charge { Name = nameCharge };

            return await _chargesAdderRepository.CreateCharge(charge);
        }
    }
}
