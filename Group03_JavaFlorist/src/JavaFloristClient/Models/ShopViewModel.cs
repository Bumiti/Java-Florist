namespace JavaFloristClient.Models
{
    public class ShopViewModel
    {
        public IEnumerable<Bouquet> Bouquets { get; set; }
        public PaginationHeader Pagination { get; set; }
        public bool HasPreviousPage => Pagination.CurrentPage > 1;
        public bool HasNextPage => Pagination.CurrentPage < Pagination.TotalPages;
    }
}
