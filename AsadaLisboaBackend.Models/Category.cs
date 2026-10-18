namespace AsadaLisboaBackend.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Foreign Key
    public ICollection<New> News { get; set; } = new List<New>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}