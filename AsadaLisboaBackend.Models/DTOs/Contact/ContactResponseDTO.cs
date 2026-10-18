using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Contact;

public class ContactResponseDto
{
    public Guid Id { get; set; }
    public string ContactType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public byte Order { get; set; }
}

public static partial class ContactExtensions
{
    public static Expression<Func<Models.Contact, ContactResponseDto>> MapContactResponseDto()
    {
        return contact => new ContactResponseDto
        {
            Id = contact.Id,
            Order = contact.Order,
            Value = contact.Value,
            ContactType = contact.ContactType,
        };
    }

    public static ContactResponseDto ToContactResponseDto(this Models.Contact contact)
    {
        return new ContactResponseDto()
        {
            Id = contact.Id,
            Order = contact.Order,
            Value = contact.Value,
            ContactType = contact.ContactType,
        };
    }
}