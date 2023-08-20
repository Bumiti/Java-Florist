using Microsoft.AspNetCore.Mvc.Rendering;

namespace JavaFloristClient.Repositories
{
    public interface IJavaFloristClientService
    {
        Task<SelectList> GetSelectList<TEntity>(string apiEndpoint, string valueField, string textField);
    }
}
