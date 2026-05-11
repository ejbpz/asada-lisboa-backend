using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsDeleterService : IContactsDeleterService
    {
        private readonly ILogger<ContactsDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IContactsDeleterRepository _contactsDeleterRepository;

        public ContactsDeleterService(IContactsDeleterRepository contactsDeleterRepository, ILogger<ContactsDeleterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _contactsDeleterRepository = contactsDeleterRepository;
        }

        public async Task DeleteContact(Guid id)
        {
            try { 

                await _contactsDeleterRepository.DeleteContact(id);

                _memoryCachesService.RemoveById(Constants.CACHE_CONTACTS, id);
                _memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

                _logger.LogInformation("Contacto con Id: {Id} eliminado exitosamente.", id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminar  contacto con Id: {Id}", id);
                throw;
            }


        }
    }
}
