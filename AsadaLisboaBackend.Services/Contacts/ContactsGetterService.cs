using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts;

public class ContactsGetterService(
    IContactsGetterRepository contactsGetterRepository,
    ILogger<ContactsGetterService> logger,
    IMemoryCachesService memoryCachesService)
    : IContactsGetterService
{
    public async Task<PageResponseDto<ContactResponseDto>> GetContacts(SearchSortRequestDto searchSortRequestDto)
    {
        try {
            var result = await memoryCachesService.GetOrCreateCacheList(
                resource: Constants.CACHE_CONTACTS,
                request: searchSortRequestDto,
                create: () => contactsGetterRepository.GetContacts(searchSortRequestDto),
                time: TimeSpan.FromMinutes(5));
                
            logger.LogInformation(
                "Obtención exitosa de configuración. Tamaño: {Take}",
                searchSortRequestDto.Take
            );

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error al obtener contacto. Tamaño: {Take}",
                searchSortRequestDto.Take
            );
            throw;
        }
    }
}