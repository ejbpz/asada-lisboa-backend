using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Repositories.Users
{
    public class UsersGetterRepository : IUsersGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PageResponseDTO<UserResponseDTO>> GetUsers(SearchSortRequestDTO searchSortRequestDTO)
        {
            IQueryable<ApplicationUser> query = _context.Users
                .AsNoTracking()
                .Include(u => u.Charge);

            // Search
            if (!string.IsNullOrEmpty(searchSortRequestDTO.Search) && !string.IsNullOrWhiteSpace(searchSortRequestDTO.Search))
            {
                string search = searchSortRequestDTO.Search.ToLower();

                query = searchSortRequestDTO.FilterBy switch
                {
                    "Charge" => query.Where(u => u.Charge != null && u.Charge.Name.ToLower().Contains(search)),
                    _ => query.Where(u => (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName) != null && (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName).ToLower().Contains(search)),
                };
            }

            // Sort
            query = (searchSortRequestDTO.SortBy.ToLower(), searchSortRequestDTO.SortDirection.ToLower()) switch
            {
                ("charge", "desc") => query.OrderByDescending(u => u.Charge!.Name),
                ("charge", _) => query.OrderBy(u => u.Charge!.Name),
                ("name", "desc") => query.OrderByDescending(u => (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName)),
                _ => query.OrderBy(u => (u.FirstName + " " + u.FirstLastName + " " + u.SecondLastName)),
            };

            return new PageResponseDTO<UserResponseDTO>()
            {
                Total = await query.CountAsync(),
                Data = await query
                    .AsNoTracking()
                    .OrderBy(u => u.Id)
                    .Skip(searchSortRequestDTO.Offset)
                    .Take(searchSortRequestDTO.Take)
                    .Select(UserExtensions.MapUserResponseDTO())
                    .ToListAsync(),
            }; 
        }

        public async Task<UserDetailResponseDTO?> GetUser(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Select(UserExtensions.MapUserDetailResponseDTO())
                .FirstOrDefaultAsync();
        }
    }
}
