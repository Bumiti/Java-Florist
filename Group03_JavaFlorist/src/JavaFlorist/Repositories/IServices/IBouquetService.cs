using JavaFlorist.Models;

namespace JavaFlorist.Repositories.IServices
{
    public interface IBouquetService
    {
        Task<List<Bouquet>> GetAllAsync(int page); // Get ALL
        Task<Bouquet> GetByIdAsync(int id, bool includes = false); //Get By Id
        Task<Bouquet> AddAsync(Bouquet entity); // POST  
        Task<Bouquet> UpdateAsync(Bouquet entity); // PUT 
        Task<(bool, string)> DeleteAsync(Bouquet entity); // DELETE 
        int CountBouquet();
    }
}
