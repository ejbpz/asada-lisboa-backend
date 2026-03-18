using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.Services.Contacts
{
    public class ContactsAdderService : IContactsAdderService
    {
        private readonly IContactsAdderRepository _contactsAdderRepository;

        public ContactsAdderService(IContactsAdderRepository contactsAdderRepository)
        {
            _contactsAdderRepository = contactsAdderRepository;
        }

        public async Task<ContactResponseDTO> CreateContact(ContactRequestDTO contactRequestDTO)
        {
            var contact = new Contact()
            {
                Order = contactRequestDTO.Order,
                Value = contactRequestDTO.Value,
                ContactType = contactRequestDTO.ContactType,
            };

            return (await _contactsAdderRepository.CreateContact(contact))
                .ToContactResponseDTO();
        }
    }
}
