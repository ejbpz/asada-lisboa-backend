using AsadaLisboaBackend.Models.DTOs.Receipt;
using AsadaLisboaBackend.ServiceContracts.Receipts;

namespace AsadaLisboaBackend.Services.Receipts;

public partial class ReceiptsGetterService : IReceiptsGetterService
{
    public Task<ReceiptResponseDto> GetReceipt(int receiptNumber)
    {
        return GetReceiptInternal(receiptNumber);
    }

    public Task<ReceiptDetailsResponseDto> GetReceiptDetails(int receiptNumber, int index)
    {
        return GetReceiptDetailsInternal(receiptNumber, index);
    }
}