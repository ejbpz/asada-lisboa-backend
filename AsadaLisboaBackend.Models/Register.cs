using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models
{
    public class Register
    {
        [Key]
        public int IdRegister { get; set; }

        [Required(ErrorMessage = "Nombre de usuario es requerido")]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Correo es requerido")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Contraseña es requerido")]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Confirmación de contraseña es requerido")]
        public string ConfirnPassword { get; set; } = string.Empty;
    }
}
