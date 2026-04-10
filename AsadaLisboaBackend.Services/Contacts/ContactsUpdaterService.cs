using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsUpdaterService : IContactsUpdaterService
    {
        private readonly IContactsUpdaterRepository _contactsUpdaterRepository;
        private readonly ILogger<ContactsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ContactsUpdaterService(IContactsUpdaterRepository contactsUpdaterRepository, ILogger<ContactsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _contactsUpdaterRepository = contactsUpdaterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<ContactResponseDTO> UpdateContact(Guid id, ContactRequestDTO contactsRequestDTO)
        {
            var result = (await _contactsUpdaterRepository.UpdateContact(id, contactsRequestDTO));

            if (result = null)
            {
                _logger.LogWarning("No se encontró contacto para actualizar con Id: {Id}", id);
            }

            _logger.LogInformation("Actualización exitosa de contacto con Id: {Id}", id);

            _memoryCachesService.RemoveById(Constants.CACHE_CONTACTS, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

            return result.ToContactResponseDTO();
        }
    }
}
