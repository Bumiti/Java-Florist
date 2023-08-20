using JavaFlorist.Models;

namespace JavaFlorist.Repositories.IServices
{
    public interface IDashBoard
    {
        Task<List<RevenueFilterByFlorist>> GetFilterByFlorists();
    }
}
