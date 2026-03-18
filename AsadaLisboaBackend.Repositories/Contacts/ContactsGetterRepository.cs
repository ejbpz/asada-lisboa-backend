using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts
{
    public class ContactsGetterRepository : IContactsGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactsGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<ContactResponseDTO>> GetContacts(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<Contact> query = _context.Contacts
                .AsNoTracking();

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = query.Where(c => (c.ContactType).ToLower().Contains(search));
            }

            // Sort
            query = (searchSortRequestDTO.SortBy.ToLower(), searchSortRequestDTO.SortDirection.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(u => u.ContactType),
                ("name", _) => query.OrderBy(u => u.ContactType),
                (_, "desc") => query.OrderByDescending(u => (u.Order)),
                _ => query.OrderBy(u => (u.Order)),
            };

            return new PageResponseDTO<ContactResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .AsNoTracking()
                    .OrderBy(u => u.Id)
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(ContactExtensions.MapContactResponseDTO())
                    .ToListAsync(),
            };
        }
    }
}
