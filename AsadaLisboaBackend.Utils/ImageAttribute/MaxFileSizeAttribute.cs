using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Utils.ImageAttribute
{
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
}
