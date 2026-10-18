using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AsadaLisboaBackend.Utils.DocumentAttribute;

public class MaxFileSizeAttribute(int maxFileSizeMb) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            // Convertir MB a Bytes
            var maxBytes = maxFileSizeMb * 1024 * 1024;

            if (file.Length > maxBytes)
            {
                return new ValidationResult($"El archivo no puede pesar más de {maxFileSizeMb} MB.");
            }
        }

        return ValidationResult.Success;
    }
}