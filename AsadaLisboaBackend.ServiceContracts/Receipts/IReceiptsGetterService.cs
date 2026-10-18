using AsadaLisboaBackend.Models.DTOs.Receipt;

namespace AsadaLisboaBackend.ServiceContracts.Receipts;

public interface IReceiptsGetterService
{
    public Task<ReceiptResponseDto> GetReceipt(int receiptNumber);
    public Task<ReceiptDetailsResponseDto> GetReceiptDetails(int receiptNumber, int index);
}