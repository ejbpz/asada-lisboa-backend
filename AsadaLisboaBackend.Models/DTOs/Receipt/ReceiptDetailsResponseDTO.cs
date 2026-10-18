namespace AsadaLisboaBackend.Models.DTOs.Receipt;

public class ReceiptDetailsResponseDto
{
    public string? Meter { get; set; } = null;
    public string? Lapse { get; set; } = null;
    public string? Address { get; set; } = null;
    public string? UserName { get; set; } = null;
    public string? ExpirationDate { get; set; } = null;

    public decimal? Total { get; set; } = null;
    public int? CubicMeters { get; set; } = null;
    public int? CurrentReading { get; set; } = null;
    public int? PreviousReading { get; set; } = null;

    public decimal? Taxes { get; set; } = null;
    public decimal? Repairs { get; set; } = null;
    public decimal? Arrears { get; set; } = null;
    public decimal? Hydrants { get; set; } = null;
    public decimal? BaseRate { get; set; } = null;
    public decimal? CreditNote { get; set; } = null;
    public decimal? Consumption { get; set; } = null;
    public decimal? FixedCharges { get; set; } = null;
    public decimal? OtherCharges { get; set; } = null;
    public decimal? WaterResource { get; set; } = null;
    public decimal? DisconnectionReconnection { get; set; } = null;
}