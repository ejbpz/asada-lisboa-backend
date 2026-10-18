using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Charges;

public class ChargesAdderService(
    IChargesGetterService chargesGetterService,
    IChargesAdderRepository chargesAdderRepository,
    ILogger<ChargesAdderService> logger,
    IMemoryCachesService memoryCachesService)
    : IChargesAdderService
{
    public async Task<ChargeResponseDto> CreateCharge(string nameCharge)
    {

        try 
        {
            var existsCharge = await chargesGetterService.ExistsCharge(nameCharge);

            if (existsCharge){

                logger.LogWarning("No se pudo crear el cargo '{ChargeName}' porque ya existe.", nameCharge);
                throw new ExistingValueException("El nombre del cargo ya existe.");
            }
            var charge = new Charge { Name = nameCharge };

               
            var result = await chargesAdderRepository.CreateCharge(charge);

            memoryCachesService.ChangeVersion(Constants.CACHE_CHARGES);

            return result;

        } catch (Exception ex) {
            logger.LogError(ex, "Error al crear el cargo '{ChargeName}'", nameCharge);
            throw new CreateObjectException("Error al crear el cargo.");
        }
    }
}