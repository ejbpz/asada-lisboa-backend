using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;

namespace AsadaLisboaBackend.RepositoryContracts.Contacts
{
    public interface IContactsGetterRepository
    {
        public Task<PageResponseDTO<ContactResponseDTO>> GetContacts(SearchSortRequestDTO searchSortRequestDTO);
    }
}
