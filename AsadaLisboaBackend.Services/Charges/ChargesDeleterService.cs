using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.Charges;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace AsadaLisboaBackend.Services.Charges
{
    public class ChargesDeleterService : IChargesDeleterService
    {
        private readonly IChargesDeleterRepository _chargesDeleterRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChargesDeleterService> _logger;

        public ChargesDeleterService(IChargesDeleterRepository chargesDeleterRepository, UserManager<ApplicationUser> userManager, ILogger<ChargesDeleterService> logger)
        {
            _chargesDeleterRepository = chargesDeleterRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task DeleteCharge(Guid id)
        {
            var charge = await _userManager.FindByIdAsync(id.ToString());

            if (charge is null)
            {
                _logger.LogWarning("Cargo con id {ChargeId} no encontrado para eliminación.", id);
                throw new NotFoundException("Usuario inexistente.");
            }

            await _chargesDeleterRepository.DeleteCharge(id);

            _logger.LogInformation("Cargo con id {ChargeId} eliminado exitosamente.", id);

        }
    }
}
