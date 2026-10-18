using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Contacts;

public class ContactsAdderService(
    IContactsAdderRepository contactsAdderRepository,
    ILogger<ContactsAdderService> logger,
    IMemoryCachesService memoryCachesService)
    : IContactsAdderService
{
    public async Task<ContactResponseDto> CreateContact(ContactRequestDto contactRequestDto)
    {
        try { 
            var contact = new Contact()
            {
                Order = contactRequestDto.Order,
                Value = contactRequestDto.Value,
                ContactType = contactRequestDto.ContactType,
            };

            var result = (await contactsAdderRepository.CreateContact(contact));

            logger.LogInformation(
                "Contacto creado exitosamente. Tipo: {ContactType}, Orden: {Order}",
                contactRequestDto.ContactType,
                contactRequestDto.Order
            );

            memoryCachesService.ChangeVersion(Constants.CACHE_CONTACTS);

            return result.ToContactResponseDto();
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error al crear contacto. Tipo: {ContactType}, Orden: {Order}",
                contactRequestDto.ContactType,
                contactRequestDto.Order
            );
            throw;
        }

    }
}