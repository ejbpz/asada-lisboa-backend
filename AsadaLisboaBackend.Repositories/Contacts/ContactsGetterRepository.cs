using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Contact;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts;

public class ContactsGetterRepository(ApplicationDbContext context) : IContactsGetterRepository
{
    public async Task<PageResponseDto<ContactResponseDto>> GetContacts(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<Contact> query = context.Contacts
            .AsNoTracking();

        // Search
        if (!string.IsNullOrEmpty(searchSortRequestDto.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDto.Search))
        {
            var search = searchSortRequestDto.Search.ToLower();

            query = query.Where(c =>
                EF.Functions.ILike(c.ContactType, $"%{search}%"));
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(u => u.ContactType),
            ("name", _) => query.OrderBy(u => u.ContactType),
            (_, "desc") => query.OrderByDescending(u => (u.Order)),
            _ => query.OrderBy(u => (u.Order)),
        };

        return new PageResponseDto<ContactResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(ContactExtensions.MapContactResponseDto())
                .ToListAsync(),
        };
    }
}