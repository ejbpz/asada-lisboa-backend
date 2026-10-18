using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;

namespace AsadaLisboaBackend.ServiceContracts.Contacts;

public interface IContactsGetterService
{
    public Task<PageResponseDto<ContactResponseDto>> GetContacts(SearchSortRequestDto searchSortRequestDto);
}