using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Receipt;
using AsadaLisboaBackend.ServiceContracts.Receipts;

namespace AsadaLisboaBackend.Areas.Cliente.Controllers;

/// <summary>
/// Controller for receipts, public access.
/// </summary>
[ApiController]
[Area("Cliente")]
[ApiVersion("1.0")]
[Route("api/[area]/[controller]")]
public class RecibosController : ControllerBase
{
    private readonly IReceiptsGetterService _receiptsGetterService;

    /// <summary>
    /// Constructor for RecibosController.
    /// </summary>
    /// <param name="receiptsGetterService">Service for getting receipts content.</param>
    public RecibosController(IReceiptsGetterService receiptsGetterService)
    {
        _receiptsGetterService = receiptsGetterService;
    }

    /// <summary>
    /// Gets a receipt by its number.
    /// </summary>
    /// <param name="receiptNumber">The number of the receipt to retrieve.</param>
    /// <returns>The requested receipt.</returns>
    [HttpPost("{receiptNumber}")]
    public async Task<ActionResult<ReceiptResponseDto>> GetReceipt([FromRoute] int receiptNumber)
    {
        return Ok(await _receiptsGetterService.GetReceipt(receiptNumber));
    }

    /// <summary>
    /// Gets the details of a receipt.
    /// </summary>
    /// <param name="receiptDetailsRequestDto">The request DTO containing the receipt number and index.</param>
    /// <returns>The details of the requested receipt.</returns>
    [HttpPost("")]
    public async Task<ActionResult<ReceiptDetailsResponseDto>> GetReceiptDetails([FromBody] ReceiptDetailsRequestDto receiptDetailsRequestDto)
    {
        return Ok(await _receiptsGetterService.GetReceiptDetails(receiptDetailsRequestDto.ReceiptNumber, receiptDetailsRequestDto.Index));
    }
}