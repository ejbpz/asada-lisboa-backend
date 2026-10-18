using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Status;

public class StatusResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static partial class StatusExtensions
{
    public static Expression<Func<Models.Status, StatusResponseDto>> MapStatusResponseDto()
    {
        return status => new StatusResponseDto
        {
            Id = status.Id,
            Name = status.Name ?? string.Empty,
        };
    }

    public static StatusResponseDto ToStatusResponseDto(this Models.Status status)
    {
        return new StatusResponseDto()
        {
            Id = status.Id,
            Name = status.Name ?? string.Empty,
        };
    }
}