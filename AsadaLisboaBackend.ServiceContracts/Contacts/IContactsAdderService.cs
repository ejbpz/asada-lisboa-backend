using AsadaLisboaBackend.Models.DTOs.Contact;

namespace AsadaLisboaBackend.ServiceContracts.Contacts
{
    public interface IContactsAdderService
    {
        public Task<ContactResponseDTO> CreateContact(ContactRequestDTO contactRequestDTO);
    }
}
