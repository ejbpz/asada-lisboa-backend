using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Charges;

public class ChargesUpdaterService(
    IChargesGetterService chargesGetterService,
    IChargesUpdaterRepository chargesUpdaterRepository,
    ILogger<ChargesUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IChargesUpdaterService
{
    public async Task<ChargeResponseDto> UpdateCharge(Guid id, string chargeRequest)
    {
        var existsCharge = await chargesGetterService.ExistsCharge(chargeRequest);


        if (existsCharge)
        {
            logger.LogWarning("Intento de actualización fallido: El nombre '{ChargeName}' ya existe.", chargeRequest);
            throw new ExistingValueException("El nombre del cargo ya existe.");
        }

        var result = await chargesUpdaterRepository.UpdateCharge(id, chargeRequest);

        logger.LogInformation("Cargo con ID: {Id} actualizado exitosamente.", id);

        memoryCachesService.RemoveById(Constants.CACHE_CHARGES, id);
        memoryCachesService.ChangeVersion(Constants.CACHE_CHARGES);

        return result;
    }
}