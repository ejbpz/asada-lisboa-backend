using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesUpdaterService : IChargesUpdaterService
    {
        private readonly IChargesGetterService _chargesGetterService;
        private readonly IChargesUpdaterRepository _chargesUpdaterRepository;
        private readonly ILogger<ChargesUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ChargesUpdaterService(IChargesGetterService chargesGetterService, IChargesUpdaterRepository chargesUpdaterRepository, ILogger<ChargesUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _chargesGetterService = chargesGetterService;
            _chargesUpdaterRepository = chargesUpdaterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
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

            _memoryCachesService.RemoveById(Constants.CACHE_CHARGES, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_CHARGES);

            return result;
        }
    }
}
