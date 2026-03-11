using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Repositories.Users
{
    public class UsersUpdaterRepository : IUsersUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateUser(ApplicationUser updateUser)
        {
            _context.Entry(updateUser).Property(x => x.FirstName).IsModified = true;
            _context.Entry(updateUser).Property(x => x.FirstLastName).IsModified = true;
            _context.Entry(updateUser).Property(x => x.SecondLastName).IsModified = true;
            _context.Entry(updateUser).Property(x => x.PhoneNumber).IsModified = true;
            _context.Entry(updateUser).Property(x => x.ImageUrl).IsModified = true;
            _context.Entry(updateUser).Property(x => x.ChargeId).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ArgumentException("The user was modified by another process.");
            }
        }
    }
}
