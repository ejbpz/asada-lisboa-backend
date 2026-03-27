using AsadaLisboaBackend.Models.DTOs.New;
using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Status
{
    public class StatusResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public static partial class StatusExtensions
    {
        public static Expression<Func<Models.Status, StatusResponseDTO>> MapStatusResponseDTO()
        {
            return status => new StatusResponseDTO
            {
                Id = status.Id,
                Name = status.Name ?? string.Empty,
            };
        }

        public static StatusResponseDTO ToStatusResponseDTO(this Models.Status status)
        {
            return new StatusResponseDTO()
            {
                Id = status.Id,
                Name = status.Name ?? string.Empty,
            };
        }
    }
}
