using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Repositories.AboutUsSections
{
    public class AboutUsSectionsAdderRepository : IAboutUsSectionsAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public AboutUsSectionsAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AboutUsSection> CreateAboutUsSection(AboutUsSection aboutUsSection)
        {
            _context.AboutUsSections.Add(aboutUsSection);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Error al crear la sección.");

            return aboutUsSection;
        }
    }
}
