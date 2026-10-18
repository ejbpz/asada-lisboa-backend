using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts;

public class ContactsUpdaterService(
    IContactsUpdaterRepository contactsUpdaterRepository,
    ILogger<ContactsUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IContactsUpdaterService
{
    public async Task<ContactResponseDto> UpdateContact(Guid id, ContactRequestDto contactsRequestDto)
    {
        var result = (await contactsUpdaterRepository.UpdateContact(id, contactsRequestDto));

        logger.LogInformation("Actualización exitosa de contacto con Id: {Id}", id);

        memoryCachesService.RemoveById(Constants.CACHE_CONTACTS, id);
        memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

        return result.ToContactResponseDto();
    }
}