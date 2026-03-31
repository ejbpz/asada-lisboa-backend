using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Repositories.Charges
{
    public class ChargesUpdaterRepository : IChargesUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChargesUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChargeResponseDTO> UpdateCharge(Guid id, string chargeRequest)
        {
            var affectedRows = await _context.Charges
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(c => c.Name, chargeRequest));

            if (affectedRows < 1)
                    throw new UpdateObjectException("Error al actualizar el cargo.");

            return new ChargeResponseDTO { Id = id, Name = chargeRequest };
        }
    }
}
