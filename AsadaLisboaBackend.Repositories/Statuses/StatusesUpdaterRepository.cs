using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Repositories.Statuses
{
    public class StatusesUpdaterRepository : IStatusesUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public StatusesUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType)
        {
            var affectedRows = objectType switch
            {
                ObjectTypeEnum.New => await _context.News
                    .Where(n => n.Id == objectId)
                    .ExecuteUpdateAsync(p => p
                        .SetProperty(n => n.StatusId, statusId)),

                ObjectTypeEnum.Image => await _context.Images
                    .Where(i => i.Id == objectId)
                    .ExecuteUpdateAsync(p => p
                        .SetProperty(i => i.StatusId, statusId)),

                ObjectTypeEnum.Document => await _context.Documents
                    .Where(d => d.Id == objectId)
                    .ExecuteUpdateAsync(p => p
                        .SetProperty(d => d.StatusId, statusId)),

                _ => 0
            };

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al actualizar el estado de la entidad.");
        }
    }
}
