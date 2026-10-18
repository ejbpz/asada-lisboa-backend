using AsadaLisboaBackend.Models.DTOs.Contact;

namespace AsadaLisboaBackend.ServiceContracts.Contacts;

public interface IContactsAdderService
{
    public Task<ContactResponseDto> CreateContact(ContactRequestDto contactRequestDto);
}