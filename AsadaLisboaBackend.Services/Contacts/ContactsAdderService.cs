using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsAdderService : IContactsAdderService
    {
        private readonly IContactsAdderRepository _contactsAdderRepository;
        private readonly ILogger<ContactsAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ContactsAdderService(IContactsAdderRepository contactsAdderRepository, ILogger<ContactsAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _contactsAdderRepository = contactsAdderRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
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
