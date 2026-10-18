using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Repositories.Users;

public class UsersGetterRepository(ApplicationDbContext context) : IUsersGetterRepository
{
    public async Task<PageResponseDto<UserResponseDto>> GetUsers(SearchSortRequestDto searchSortRequestDto)
    {
        IQueryable<ApplicationUser> query = context.Users
            .AsNoTracking()
            .Include(u => u.Charge);

        if (searchSortRequestDto.IsPublic)
        {
            query = query.Where(u =>
                u.Charge != null &&
                u.Charge.Name.Trim().ToLower() != "administrador");
        }

        if (searchSortRequestDto.IsPublic && searchSortRequestDto.FilterBy?.Trim().ToLower() == "charge")
        {
            searchSortRequestDto.FilterBy = null;
        }

        // Search
        if (!string.IsNullOrEmpty(searchSortRequestDto.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDto.Search))
        {
            string search = searchSortRequestDto.Search.ToLower();

            query = searchSortRequestDto.FilterBy switch
            {
                "charge" => query.Where(u =>
                    EF.Functions.ILike(u.Charge!.Name, $"%{search}%")),

                _ => query.Where(u =>
                    EF.Functions.ILike((u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName), $"%{search}%")),
            };
        }

        // Sort
        query = (searchSortRequestDto.SortBy.ToLower(), searchSortRequestDto.SortDirection.ToLower()) switch
        {
            ("charge", "desc") => query.OrderByDescending(u => u.Charge!.Name),
            ("charge", _) => query.OrderBy(u => u.Charge!.Name),
            ("name", "desc") => query.OrderByDescending(u => (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName)),
            _ => query.OrderBy(u => (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName)),
        };

        return new PageResponseDto<UserResponseDto>()
        {
            Total = await query.CountAsync(),
            Data = await query
                .Skip(searchSortRequestDto.Offset)
                .Take(searchSortRequestDto.Take)
                .Select(UserExtensions.MapUserResponseDto())
                .ToListAsync(),
        }; 
    }

    public async Task<UserDetailResponseDto?> GetUser(Guid id)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(u => u.Charge)
            .Where(u => u.Id == id)
            .Select(UserExtensions.MapUserDetailResponseDto())
            .FirstOrDefaultAsync();

        if (user is null) 
            return null;

        user.Roles = await context.UserRoles
            .Where(ur => ur.UserId == id)
            .Join(
                context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r
            )
            .Select(RoleExtensions.MapRoleResponseDto())
            .ToListAsync();

        return user;
    }
}