using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges
{
    public class ChargesDeleterRepository : IChargesDeleterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChargesDeleterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteCharge(Guid id)
        {
            var affectedRows = await _context.Charges
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Error al eliminar el cargo.");
        }
    }
}
