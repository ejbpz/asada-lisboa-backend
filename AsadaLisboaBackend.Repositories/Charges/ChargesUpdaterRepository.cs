using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Repositories.Charges
{
    public class ChargesUpdaterRepository : IChargesUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public ChargesUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
