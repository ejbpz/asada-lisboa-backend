using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;
using Microsoft.Extensions.Logging;

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
                  searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

                  var result = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<ContactResponseDTO>>
                    (
                      resource: Constants.CACHE_CONTACTS,
                      request: searchSortRequestDTO,
                      create: () => _contactsGetterRepository.GetContacts(searchSortRequestDTO),
                      time: TimeSpan.FromMinutes(5));
                

                _logger.LogInformation(
                    "Obtención exitosa de configuración. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener contacto. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );
                throw;
            }
        }
    }
}
