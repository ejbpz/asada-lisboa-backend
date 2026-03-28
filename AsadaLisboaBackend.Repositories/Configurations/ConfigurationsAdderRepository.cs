using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Repositories.Configurations
{
    public class ConfigurationsAdderRepository : IConfigurationsAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationsAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VisualSetting> CreateConfiguration(VisualSetting visualSetting)
        {
            _context.VisualSettings.Add(visualSetting);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Error al crear la configuración.");

            return visualSetting;
        }
    }
}
