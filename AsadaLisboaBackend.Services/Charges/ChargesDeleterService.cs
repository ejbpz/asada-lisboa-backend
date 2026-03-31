using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesDeleterService : IChargesDeleterService
    {
        private readonly IChargesDeleterRepository _chargesDeleterRepository;

        public ChargesDeleterService(IChargesDeleterRepository chargesDeleterRepository)
        {
            _chargesDeleterRepository = chargesDeleterRepository;
        }

        public async Task DeleteCharge(Guid id)
        {
            await _chargesDeleterRepository.DeleteCharge(id);
        }
    }
}
