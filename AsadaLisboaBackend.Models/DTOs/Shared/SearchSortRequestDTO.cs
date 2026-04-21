using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Models.DTOs.Shared
{
    public class SearchSortRequestDTO
    {
        public string? Search { get; set; }
        public string? FilterBy { get; set; }
        public int Offset { get; set; } = 0;
        public string SortBy { get; set; } = "name";
        public string SortDirection { get; set; } = "asc";
        public int Take { get; set; } = Constants.PAGINATION_SIZE;
    }
}
