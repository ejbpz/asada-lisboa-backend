using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts;

public class ContactsDeleterService(
    IContactsDeleterRepository contactsDeleterRepository,
    ILogger<ContactsDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : IContactsDeleterService
{
    public async Task DeleteContact(Guid id)
    {
        try { 
            await contactsDeleterRepository.DeleteContact(id);

            memoryCachesService.RemoveById(Constants.CACHE_CONTACTS, id);
            memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

            logger.LogInformation("Contacto con Id: {Id} eliminado exitosamente.", id);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error eliminar  contacto con Id: {Id}", id);
            throw;
        }
    }
}