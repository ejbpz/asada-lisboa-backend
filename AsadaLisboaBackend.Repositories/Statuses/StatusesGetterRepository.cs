using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Repositories.Statuses
{
    public class StatusesGetterRepository : IStatusesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StatusResponseDTO>> GetStatuses()
        {
            return await _context.Statuses
                .Select(StatusExtensions.MapStatusResponseDTO())
                .ToListAsync();
        }

        public async Task<StatusResponseDTO> GetStatus(Guid id)
        {
            var status = await _context.Statuses
                .Select(StatusExtensions.MapStatusResponseDTO())
                .FirstOrDefaultAsync(s => s.Id == id);

            if (status is null)
                throw new NotFoundException("No existe el estado seleccionado.");

            return status;
        }
    }
}
