using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models
{
    public class Categories
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Name { get; set; } = string.Empty;
    }
}
