using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models.DTOs.AboutUs;

public class AboutUsRequestDto
{
    [Required(ErrorMessage = "El nombre de la sección es requerido.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre de la sección debe ser entre {1} y {0} caracteres.")]
    public string SectionType { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido de la sección es requerido.")]
    [StringLength(2500, MinimumLength = 50, ErrorMessage = "El contenido de la sección debe ser entre {1} y {0} caracteres.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "El orden de la sección es requerido.")]
    [Range(0, 255, ErrorMessage = "El orden de la sección debe ser entre {0} y {1} caracteres.")]
    public byte Order { get; set; } = 0;
}