using AsadaLisboaBackend.Models.DTOs.Contact;

namespace AsadaLisboaBackend.ServiceContracts.Contacts;

public interface IContactsUpdaterService
{
    public Task<ContactResponseDto> UpdateContact(Guid id, ContactRequestDto contactsRequestDto);
}