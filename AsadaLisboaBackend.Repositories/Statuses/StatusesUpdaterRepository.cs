using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Repositories.Statuses;

public class StatusesUpdaterRepository(ApplicationDbContext context) : IStatusesUpdaterRepository
{
    public async Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType)
    {
        var affectedRows = objectType switch
        {
            ObjectTypeEnum.New => await context.News
                .Where(n => n.Id == objectId)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(n => n.StatusId, statusId)),

            ObjectTypeEnum.Image => await context.Images
                .Where(i => i.Id == objectId)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(i => i.StatusId, statusId)),

            ObjectTypeEnum.Document => await context.Documents
                .Where(d => d.Id == objectId)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(d => d.StatusId, statusId)),

            _ => 0
        };

        if (affectedRows < 1)
            throw new UpdateObjectException("Error al actualizar el estado de la entidad.");
    }
}