using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsGetterService : IContactsGetterService
    {
        private readonly IContactsGetterRepository _contactsGetterRepository;
        private readonly ILogger<ContactsGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ContactsGetterService(IContactsGetterRepository contactsGetterRepository, ILogger<ContactsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _contactsGetterRepository = contactsGetterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<PageResponseDTO<ContactResponseDTO>> GetContacts(SearchSortRequestDTO searchSortRequestDTO)
        {
            try {
                  var result = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>
                    (
                      resource: Constants.CACHE_CONTACTS,
                      request: searchSortRequestDTO,
                      create: () => _contactsGetterRepository.GetContacts(searchSortRequestDTO),
                      time: TimeSpan.FromMinutes(5));
                

                _logger.LogInformation(
                    "Obtención exitosa de configuración. Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener contacto. Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );
                throw;
            }
        }
    }
}
