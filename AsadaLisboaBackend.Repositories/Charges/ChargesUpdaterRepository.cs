using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Repositories.Charges;

public class ChargesUpdaterRepository(ApplicationDbContext context) : IChargesUpdaterRepository
{
    public async Task<ChargeResponseDto> UpdateCharge(Guid id, string chargeRequest)
    {
        var affectedRows = await context.Charges
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(c => c.Name, chargeRequest));

        return affectedRows < 1 ? throw new UpdateObjectException("Error al actualizar el cargo.") : new ChargeResponseDto { Id = id, Name = chargeRequest };
    }
}