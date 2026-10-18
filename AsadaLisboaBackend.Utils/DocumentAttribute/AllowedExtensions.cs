using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AsadaLisboaBackend.Utils.DocumentAttribute;

public class AllowedExtensions(string[] extensions) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!extensions.Contains(extension))
                return new ValidationResult("Formato de documento no permitido.");
        }
        return ValidationResult.Success;
    }
}