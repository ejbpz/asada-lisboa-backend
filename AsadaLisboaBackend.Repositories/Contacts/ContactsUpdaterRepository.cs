using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts
{
    public class ContactsUpdaterRepository : IContactsUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactsUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Contact> UpdateContact(Guid id, ContactRequestDTO contactRequestDTO)
        {
            var affectedRows = await _context.Contacts
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(c => c.ContactType, contactRequestDTO.ContactType)
                    .SetProperty(c => c.Value, contactRequestDTO.Value)
                    .SetProperty(c => c.Order, contactRequestDTO.Order));

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar el contacto.");

            return new Contact()
            {
                Id = id,
                Order = contactRequestDTO.Order,
                Value = contactRequestDTO.Value,
                ContactType = contactRequestDTO.ContactType,
            };
        }
    }
}
