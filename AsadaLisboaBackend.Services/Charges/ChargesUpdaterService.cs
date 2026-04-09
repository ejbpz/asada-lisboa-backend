using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesUpdaterService : IChargesUpdaterService
    {
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesUpdaterRepository _chargesUpdaterRepository;
        private readonly ILogger<ChargesUpdaterService> _logger;

        public ChargesUpdaterService(IChargesGetterService chargesGetterService, IChargesUpdaterRepository chargesUpdaterRepository, ILogger<ChargesUpdaterService> logger)
        {
            _chargesGetterService = chargesGetterService;
            _chargesUpdaterRepository = chargesUpdaterRepository;
            _logger = logger;
        }

        public async Task<ChargeResponseDTO> UpdateCharge(Guid id, string chargeRequest)
        {
            var existsCharge = await _chargesGetterService.ExistsCharge(chargeRequest);


            if (existsCharge)
            {
                _logger.LogWarning("Intento de actualización fallido: El nombre '{ChargeName}' ya existe.", chargeRequest);
                throw new ExistingValueException("El nombre del cargo ya existe.");
            }

            var result = await _chargesUpdaterRepository.UpdateCharge(id, chargeRequest);

            _logger.LogInformation("Cargo con ID: {Id} actualizado exitosamente.", id);

            return result;
        }
    }
}
