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
            _context.Attach(updateUser);
            var entry = _context.Entry(updateUser);

            entry.Property(x => x.ChargeId).IsModified = true;
            entry.Property(x => x.ImageUrl).IsModified = true;
            entry.Property(x => x.FirstName).IsModified = true;
            entry.Property(x => x.PhoneNumber).IsModified = true;
            entry.Property(x => x.FirstLastName).IsModified = true;
            entry.Property(x => x.SecondLastName).IsModified = true;

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows == 0)
                throw new ArgumentException("Usuario no existente");
        }
    }
}
