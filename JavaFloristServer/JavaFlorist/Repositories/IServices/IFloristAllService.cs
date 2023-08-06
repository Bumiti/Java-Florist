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
        Task<List<Bouquet>> GetBouquetAsync(int floristId); // Get ALL
        Task<Bouquet> GetBouquetByIdAsync(int id, int floristId, bool includes = false); //Get By Id
        Task<Bouquet> AddBouquetAsync(Bouquet entity, int floristId); // POST  
        Task<Bouquet> UpdateBouquetAsync(Bouquet entity, int floristId); // PUT 
        Task<(bool, string)> DeleteBouquetAsync(Bouquet entity, int floristId); // DELETE 
        #endregion

        #region Blog
        Task<List<Blog>> GetBlogAsync(int userId); // Get ALL
        Task<Blog> GetBlogByIdAsync(int id, int userId, bool includes = false); //Get By Id
        Task<Blog> AddBlogAsync(Blog entity, int userId); // POST  
        Task<Blog> UpdateBlogAsync(Blog entity, int userId); // PUT 
        Task<(bool, string)> DeleteBlogAsync(Blog entity, int userId); // DELETE 
        #endregion

        #region Order
        Task<List<Order>> GetOrderAsync(int floristId); // Get ALL
        Task<Order> GetOrderByIdAsync(int id, int floristId, bool includes = false); //Get By Id
        Task<Order> AddOrderAsync(Order entity, int floristId); // POST  
        Task<Order> UpdateOrderAsync(Order entity, int floristId); // PUT 
        Task<(bool, string)> DeleteOrderAsync(Order entity, int floristId); // DELETE 
        Task<Order> UpdateOrderStatusDeliAsync(Order entity); // PUT 
        Task<Order> UpdateOrderStatusCancelAsync(Order entity); // PUT 
        #endregion
    }
}
