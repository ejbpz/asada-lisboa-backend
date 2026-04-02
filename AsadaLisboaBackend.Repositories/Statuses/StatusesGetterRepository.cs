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
                .AsNoTracking()
                .Select(StatusExtensions.MapStatusResponseDTO())
                .ToListAsync();
        }

        public async Task<StatusResponseDTO> GetStatus(Guid id)
        {
            var status = await _context.Statuses
                .AsNoTracking()
                .Select(StatusExtensions.MapStatusResponseDTO())
                .FirstOrDefaultAsync(s => s.Id == id);

            if (status is null)
                throw new NotFoundException("No existe el estado seleccionado.");

            return status;
        }

        public async Task<Guid> GetStatusPublicado()
        {
            Guid statusId = await _context.Statuses
                .AsNoTracking()
                .Where(s => s.Name == "Publicado")
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (statusId == Guid.Empty)
            {
                var status = await GetStatus(Guid.Parse("5C1CEBDA-FC8C-44AC-997C-AAF015572D46"));

                if (status is null)
                    return Guid.Empty;

                statusId = status.Id;
            }

            return statusId;
        }
    }
}
