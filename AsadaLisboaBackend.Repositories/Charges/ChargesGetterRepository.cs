using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges
{
    public class ChargesGetterRepository : IChargesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChargesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsCharge(string name)
        {
            return await _context.Charges
                .AsNoTracking()
                .AnyAsync(c => c.Name.Trim().ToLower() == name);
        }

        public async Task<ChargeResponseDTO> GetCharge(Guid id)
        {
            var charge = await _context.Charges
                .AsNoTracking()
                .Select(ChargeExtensions.MapChargeResponseDTO())
                .FirstOrDefaultAsync(c => c.Id == id);

            if (charge is null)
                throw new NotFoundException("No existe el cargo seleccionado.");

            return charge;
        }

        public async Task<List<ChargeResponseDTO>> GetCharges()
        {
            return await _context.Charges
                .AsNoTracking()
                .Select(ChargeExtensions.MapChargeResponseDTO())
                .ToListAsync();
        }
    }
}
