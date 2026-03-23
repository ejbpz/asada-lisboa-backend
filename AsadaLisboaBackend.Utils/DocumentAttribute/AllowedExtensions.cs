using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Utils.DocumentAtribute
{
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
}
