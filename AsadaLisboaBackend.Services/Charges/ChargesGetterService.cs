using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Charges;

public class ChargesGetterService(
    IChargesGetterRepository chargesGetterRepository,
    ILogger<ChargesUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IChargesGetterService
{
    public async Task<List<ChargeResponseDto>> GetCharges()
    {
        try
        {
            return await memoryCachesService.GetOrCreateCache<List<ChargeResponseDto>>(
                key: Constants.CACHE_CHARGES,
                create: () => chargesGetterRepository.GetCharges(),
                time: TimeSpan.FromMinutes(5));

        } catch (Exception ex) 
        {
            logger.LogError(ex, "Ocurrió un error al intentar obtener la lista de cargos.");
            throw new CreateObjectException("Error al obtener la lista de cargos.");
        }
    }

    public async Task<bool> ExistsCharge(string name)
    {
        name = name.Trim().ToLower();
        return await chargesGetterRepository.ExistsCharge(name);
    }
}