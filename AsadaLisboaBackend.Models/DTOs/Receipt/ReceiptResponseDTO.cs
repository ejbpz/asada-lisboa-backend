using AsadaLisboaBackend.Models.Enums;

namespace AsadaLisboaBackend.Models.DTOs.Receipt;

public class ReceiptResponseDto
{
    public string? UserName { get; set; } = null;
    public string? Chart1Url { get; set; } = null;
    public string? Chart2Url { get; set; } = null;
    public List<Dictionary<string, string>>? Table { get; set; }
    public ReceiptType ReceiptType { get; set; } = ReceiptType.NotExists;
}