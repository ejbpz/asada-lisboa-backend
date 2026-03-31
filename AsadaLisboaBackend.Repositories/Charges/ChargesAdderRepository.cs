using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Charge;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges
{
    public class ChargesAdderRepository : IChargesAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public ChargesAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChargeResponseDTO> CreateCharge(Charge charge)
        {
            _context.Charges.Add(charge);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al crear el cargo.");

            return charge.ToChargeResponseDTO();
        }
    }
}
