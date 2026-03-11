namespace AsadaLisboaBackend.Models.DTOs.Users
{
    public class UserUpdateRequestDTO
    {
        public Guid ChargeId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string FirstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
    }
}
