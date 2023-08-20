namespace JavaFlorist.Repositories.IServices
{
    public interface IAllService<T> where T : class
    {
        Task<List<T>> GetAllAsync(); // Get ALL
        Task<T> GetByIdAsync(int id, bool includes = false); //Get By Id
        Task<T> AddAsync(T entity); // POST  
        Task<T> UpdateAsync(T entity); // PUT 
        Task<(bool, string)> DeleteAsync(T entity); // DELETE 
    }

}
