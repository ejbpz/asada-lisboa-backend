using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Contact
{
    public class ContactResponseDTO
    {
        public Guid Id { get; set; }
        public string ContactType { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public byte Order { get; set; }
    }

    public static partial class ContactExtensions
    {
        public static Expression<Func<Models.Contact, ContactResponseDTO>> MapContactResponseDTO()
        {
            return contact => new ContactResponseDTO
            {
                Id = contact.Id,
                Order = contact.Order,
                Value = contact.Value,
                ContactType = contact.ContactType,
            };
        }

        public static ContactResponseDTO ToContactResponseDTO(this Models.Contact contact)
        {
            return new ContactResponseDTO()
            {
                Id = contact.Id,
                Order = contact.Order,
                Value = contact.Value,
                ContactType = contact.ContactType,
            };
        }
    }
}
