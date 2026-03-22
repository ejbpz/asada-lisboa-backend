using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsadaLisboaBackend.Models.DTOs.Image
{
    public class ImageResponseDTO
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }

        public long FileSize { get; set; }

        public string StatusName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }
}
