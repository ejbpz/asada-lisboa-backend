using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.Contacts
{
    public interface IContactsGetterService
    {
        public Task<PageResponseDTO<ContactResponseDTO>> GetContacts(SearchSortRequestDTO searchSortRequestDTO);
    }
}
