using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.ServiceContracts.Contacts;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsGetterService : IContactsGetterService
    {
        private readonly IContactsGetterService _contactsGetterRepository;

        public ContactsGetterService(IContactsGetterService contactsGetterRepository)
        {
            _contactsGetterRepository = contactsGetterRepository;
        }

        public async Task<PageResponseDTO<ContactResponseDTO>> GetContacts(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _contactsGetterRepository.GetContacts(searchSortRequestDTO);
        }
    }
}
