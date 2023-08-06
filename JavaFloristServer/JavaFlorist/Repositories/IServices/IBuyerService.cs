using JavaFlorist.Models;

namespace JavaFlorist.Repositories.IServices
{
    public interface IBuyerService
    {
        //View Product
        Task<List<Bouquet>> GetAllAsync(string? search, double? from, double? to, string? sortBy, int page = 1); // Get ALL
        Task<Bouquet> GetByIdAsync(int id, bool includes = false); //Get By Id
        //AddToCart
        Task<List<Cart>> GetCartItems();
        Task<List<Cart>> AddToCart(Bouquet id);
        Task<List<Cart>> RemoveFromCart(int id);
        Task<List<Cart>> DeleteFormCart(int id);
        //Create Order/Check Out
        Task<CheckOut> CreateOrder(CheckOut model);
        //Payment
        //SendMail
        //SetStatus nm
        //Discount nm
    }
}
