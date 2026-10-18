using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges;

public class ChargesAdderRepository(ApplicationDbContext context) : IChargesAdderRepository
{
    public async Task<ChargeResponseDto> CreateCharge(Charge charge)
    {
        context.Charges.Add(charge);
        var affectedRows = await context.SaveChangesAsync();

        if (affectedRows < 1)
            throw new CreateObjectException("Error al crear el cargo.");

        return charge.ToChargeResponseDto();
    }
}