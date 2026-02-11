using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
