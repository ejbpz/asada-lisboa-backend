using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsAdderService : IContactsAdderService
    {
        private readonly ILogger<ContactsAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IContactsAdderRepository _contactsAdderRepository;

        public ContactsAdderService(IContactsAdderRepository contactsAdderRepository, ILogger<ContactsAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _contactsAdderRepository = contactsAdderRepository;
        }

        public async Task<ContactResponseDTO> CreateContact(ContactRequestDTO contactRequestDTO)
        {
            try { 
                var contact = new Contact()
                {
                    Order = contactRequestDTO.Order,
                    Value = contactRequestDTO.Value,
                    ContactType = contactRequestDTO.ContactType,
                };

                var result = (await _contactsAdderRepository.CreateContact(contact));

                _logger.LogInformation(
                  "Contacto creado exitosamente. Tipo: {ContactType}, Orden: {Order}",
                  contactRequestDTO.ContactType,
                  contactRequestDTO.Order
              );

                _memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

                return result.ToContactResponseDTO();
            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al crear contacto. Tipo: {ContactType}, Orden: {Order}",
                    contactRequestDTO.ContactType,
                    contactRequestDTO.Order
                );
                throw;
            }

        }
    }
}
