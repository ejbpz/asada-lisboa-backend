using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesUpdaterService : IChargesUpdaterService
    {
        private readonly IChargesUpdaterRepository _chargesUpdaterRepository;

        public ChargesUpdaterService(IChargesUpdaterRepository chargesUpdaterRepository)
        {
            _chargesUpdaterRepository = chargesUpdaterRepository;
        }
    }
}
