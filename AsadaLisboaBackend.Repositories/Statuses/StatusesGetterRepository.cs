using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Repositories.Statuses;

public class StatusesGetterRepository(ApplicationDbContext context) : IStatusesGetterRepository
{
    public async Task<List<StatusResponseDto>> GetStatuses()
    {
        return await context.Statuses
            .AsNoTracking()
            .Select(StatusExtensions.MapStatusResponseDto())
            .ToListAsync();
    }

    public async Task<StatusResponseDto> GetStatus(Guid id)
    {
        var status = await context.Statuses
            .AsNoTracking()
            .Select(StatusExtensions.MapStatusResponseDto())
            .FirstOrDefaultAsync(s => s.Id == id);

        return status ?? throw new NotFoundException("No existe el estado seleccionado.");
    }
}