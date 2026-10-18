using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.Receipt;

public class ReceiptDetailsRequestDto
{
    [Required(ErrorMessage = "El número de recibo es requerido.")]
    public int ReceiptNumber { get; set; }

    [Required(ErrorMessage = "El índice de recibo es requerido.")]
    public int Index { get; set; }
}