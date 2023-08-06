namespace JavaFlorist.Repositories.IServices
{
    public interface IAccountService //Su dung Identity
    {
        Task<string> GetMyName();
    }
}
