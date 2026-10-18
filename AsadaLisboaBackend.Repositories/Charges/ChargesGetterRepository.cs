using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges;

public class ChargesGetterRepository(ApplicationDbContext context) : IChargesGetterRepository
{
    public async Task<bool> ExistsCharge(string name)
    {
        return await context.Charges
            .AsNoTracking()
            .AnyAsync(c => c.Name.Trim().ToLower() == name);
    }

    public async Task<ChargeResponseDto> GetCharge(Guid id)
    {
        var charge = await context.Charges
            .AsNoTracking()
            .Select(ChargeExtensions.MapChargeResponseDto())
            .FirstOrDefaultAsync(c => c.Id == id);

        if (charge is null)
            throw new NotFoundException("No existe el cargo seleccionado.");

        return charge;
    }

    public async Task<List<ChargeResponseDto>> GetCharges()
    {
        return await context.Charges
            .AsNoTracking()
            .Select(ChargeExtensions.MapChargeResponseDto())
            .ToListAsync();
    }
}