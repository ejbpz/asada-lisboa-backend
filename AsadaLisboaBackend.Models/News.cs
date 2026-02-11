using AsadaLisboaBackend.Utils.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsadaLisboaBackend.Models
{
    public class News
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(10)]
        public string urlImage { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(25)]
        public string Description { get; set; } = string.Empty;

        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;
        public DateTime LastEditionDate { get; set; }
        public Guid? CategoriesId { get; set; }

        [ForeignKey("CategoriesId")]
        public List<Categories>? Categories { get; set; } = new List<Categories>();

        public Status Status { get; set; } = Status.Pending;
    }
}
