namespace AsadaLisboaBackend.Models.DTOs.Category
{
    public class CategoryRequestDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
