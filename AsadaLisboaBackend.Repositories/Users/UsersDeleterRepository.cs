using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Repositories.Users
{
    public class UsersDeleterRepository : IUsersDeleterRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersDeleterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = new ApplicationUser() { Id = id };

            _context.Entry(user).State = EntityState.Deleted;


            var affectedRows = await _context.SaveChangesAsync();

            if(affectedRows == 0)
                throw new ArgumentException("Usuario no existente");
        }
    }
}
