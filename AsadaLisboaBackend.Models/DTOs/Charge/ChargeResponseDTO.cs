using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Charge
{
    public class ChargeResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public static partial class ChargeExtensions
    {
        public static Expression<Func<Models.Charge, ChargeResponseDTO>> MapChargeResponseDTO()
        {
            return charge => new ChargeResponseDTO
            {
                Id = charge.Id,
                Name = charge.Name ?? string.Empty,
            };
        }

        public static ChargeResponseDTO ToChargeResponseDTO(this Models.Charge charge)
        {
            return new ChargeResponseDTO()
            {
                Id = charge.Id,
                Name = charge.Name ?? string.Empty,
            };
        }
    }
}
