using JavaFlorist.Models;

namespace JavaFlorist.Repositories.IServices
{
    public interface IFloristAllService
    {
        /*  1. Bouquet
            2. Order
            3. Blog
        */
        #region Bouquet
        Task<List<Bouquet>> GetBouquetAsync(int page); // Get ALL
        Task<int> CountBouquetAsync();
        Task<Bouquet> GetBouquetByIdAsync(int id, bool includes = false); //Get By Id
        Task<Bouquet> AddBouquetAsync(Bouquet entity); // POST  
        Task<Bouquet> UpdateBouquetAsync(Bouquet entity); // PUT 
        Task<(bool, string)> DeleteBouquetAsync(Bouquet entity); // DELETE 
        #endregion

        #region Blog
        Task<List<Blog>> GetBlogAsync(int page); // Get ALL
        Task<int> CountBlogAsync();

        Task<Blog> GetBlogByIdAsync(int id, bool includes = false); //Get By Id
        Task<Blog> AddBlogAsync(Blog entityd); // POST  
        Task<Blog> UpdateBlogAsync(Blog entity); // PUT 
        Task<(bool, string)> DeleteBlogAsync(Blog entity); // DELETE 
        #endregion

        #region Order
        /*Task<List<Order>> GetOrderAsync(int floristId); // Get ALL
        Task<Order> GetOrderByIdAsync(int id, int floristId, bool includes = false); //Get By Id
        Task<Order> AddOrderAsync(Order entity, int floristId); // POST  
        Task<Order> UpdateOrderAsync(Order entity, int floristId); // PUT 
        Task<(bool, string)> DeleteOrderAsync(Order entity, int floristId); // DELETE 
        Task<Order> UpdateOrderStatusDeliAsync(Order entity); // PUT 
        Task<Order> UpdateOrderStatusCancelAsync(Order entity); // PUT */
        Task<List<Order>> GetOrderAsync(); //et ALL
        Task<List<OrderDetail>> GetOrderDetailsAsync(); //et ALL
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<Order> GetOrderByIdAsync(int id, bool includes = false); //Get By Id
        Task<Order> UpdateOrderStatusDeliAsync(Order entity); // PUT 
        Task<Order> UpdateOrderStatusCancelAsync(Order entity); // PUT
        #endregion
    }
}
