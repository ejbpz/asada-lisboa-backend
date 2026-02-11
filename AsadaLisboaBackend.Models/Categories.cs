using System.ComponentModel.DataAnnotations;

namespace AsadaLisboaBackend.Models
{
    public class Categories
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string Name { get; set; } = string.Empty;
    }
}
