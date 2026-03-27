using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.Status
{
    public class StatusChangeRequestDTO
    {
        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public Guid StatusId { get; set; }
    }
}
