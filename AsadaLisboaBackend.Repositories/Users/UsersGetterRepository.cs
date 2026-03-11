using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Users;
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

        public async Task<List<UserResponseDTO>?> GetUsers(int offset, int take)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .Skip(offset)
                .Take(take)
                .Select(UserExtensions.MapUserResponseDTO())
                .ToListAsync();
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
