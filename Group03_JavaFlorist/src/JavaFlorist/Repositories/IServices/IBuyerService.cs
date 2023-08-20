using JavaFlorist.Models;

namespace JavaFlorist.Repositories.IServices
{
    public interface IBuyerService
    {
        //View Product
        Task<List<Bouquet>> GetAllAsync(string? search, double? from, double? to, string? sortBy, int page_size, int page = 1); // Get ALL
        Task<List<Bouquet>> GetBouquetAsync(string? search, double? from, double? to, string? sortBy, string? occasion, int page = 1); // Get ALL
        Task<List<Blog>> GetBlogAsync(int page = 1); // Get ALL
        Task<Blog> GetBlogByIdAsync(int id, bool includes = false); // Get ALL
        Task<Bouquet> GetByIdAsync(int id, bool includes = false); //Get By Id
        //AddToCart
        Task<List<Cart>> GetCartItems();
        Task<Cart> GetCartItemsById(int id);
        Task<List<Cart>> AddToCart(Bouquet id);
        Task<Cart> UpdateCart(Cart cart);
        void ClearCart();
        Task<List<Cart>> DeleteFormCart(int id);
        //Create Order/Check Out
        Task<CheckOut> CreateOrder(CheckOut model);
        Task<List<Order>> GetOrder();
        Task<List<OrderDetail>> GetOrderDetails(int id);
        int CountCart();
        int CountBouquet();
        int CountBlog();
        Task<List<Occasion>> GetMessByOccasionAsync(int occasion); // Get ALL

        //Payment
        //SendMail
        //SetStatus nm
        //Discount nm
    }
}
