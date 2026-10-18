using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Charge;

public class ChargeResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static partial class ChargeExtensions
{
    public static Expression<Func<Models.Charge, ChargeResponseDto>> MapChargeResponseDto()
    {
        return charge => new ChargeResponseDto
        {
            Id = charge.Id,
            Name = charge.Name ?? string.Empty,
        };
    }

    public static ChargeResponseDto ToChargeResponseDto(this Models.Charge charge)
    {
        return new ChargeResponseDto()
        {
            Id = charge.Id,
            Name = charge.Name ?? string.Empty,
        };
    }
}