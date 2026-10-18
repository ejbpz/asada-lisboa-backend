using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts;

public class ContactsUpdaterRepository(ApplicationDbContext context) : IContactsUpdaterRepository
{
    public async Task<Contact> UpdateContact(Guid id, ContactRequestDto contactRequestDto)
    {
        var affectedRows = await context.Contacts
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(p => p
                .SetProperty(c => c.ContactType, contactRequestDto.ContactType)
                .SetProperty(c => c.Value, contactRequestDto.Value)
                .SetProperty(c => c.Order, contactRequestDto.Order));

        if (affectedRows < 1)
            throw new UpdateObjectException("Error al modificar el contacto.");

        return new Contact()
        {
            Id = id,
            Order = contactRequestDto.Order,
            Value = contactRequestDto.Value,
            ContactType = contactRequestDto.ContactType,
        };
    }
}