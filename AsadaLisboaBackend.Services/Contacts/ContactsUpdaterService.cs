using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsUpdaterService : IContactsUpdaterService
    {
        private readonly ILogger<ContactsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IContactsUpdaterRepository _contactsUpdaterRepository;

        public ContactsUpdaterService(IContactsUpdaterRepository contactsUpdaterRepository, ILogger<ContactsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _contactsUpdaterRepository = contactsUpdaterRepository;
        }

        public async Task<ContactResponseDTO> UpdateContact(Guid id, ContactRequestDTO contactsRequestDTO)
        {
            var result = (await _contactsUpdaterRepository.UpdateContact(id, contactsRequestDTO));

            if (result is null)
            {
                _logger.LogWarning("No se encontró contacto para actualizar con Id: {Id}", id);
                throw new NotFoundException($"No se encontró contacto para actualizar con Id: {id}");
            }

            _logger.LogInformation("Actualización exitosa de contacto con Id: {Id}", id);

            _memoryCachesService.RemoveById(Constants.CACHE_CONTACTS, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

            return result.ToContactResponseDTO();
        }
    }
}
