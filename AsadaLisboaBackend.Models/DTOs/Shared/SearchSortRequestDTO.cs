using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.Shared;

public class SearchSortRequestDto
{
    public string? Search { get; set; }
    public string? FilterBy { get; set; }
    public int Offset { get; set; } = 0;
    public string SortBy { get; set; } = "date";
    public string SortDirection { get; set; } = "desc";
    public int Take { get; set; } = Constants.PAGINATION_SIZE;

    public bool IsPublic { get; set; }
}