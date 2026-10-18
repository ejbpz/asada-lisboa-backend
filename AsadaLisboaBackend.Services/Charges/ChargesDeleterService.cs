using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Charges;

public class ChargesDeleterService(
    IChargesDeleterRepository chargesDeleterRepository,
    UserManager<ApplicationUser> userManager,
    ILogger<ChargesDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : IChargesDeleterService
{
    public async Task DeleteCharge(Guid id)
    {
        var charge = await userManager.FindByIdAsync(id.ToString());

        if (charge is null)
        {
            logger.LogWarning("Cargo con id {ChargeId} no encontrado para eliminación.", id);
            throw new NotFoundException("Usuario inexistente.");
        }

        await chargesDeleterRepository.DeleteCharge(id);

        memoryCachesService.RemoveById(Constants.CACHE_CHARGES, id);
        memoryCachesService.ChangeVersion(Constants.CACHE_CHARGES);


        logger.LogInformation("Cargo con id {ChargeId} eliminado exitosamente.", id);

    }
}