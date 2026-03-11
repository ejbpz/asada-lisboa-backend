using Microsoft.AspNetCore.Identity;

namespace AsadaLisboaBackend.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string FirstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public Guid ChargeId { get; set; }
        public Charge? Charge { get; set; } = null;
    }
}