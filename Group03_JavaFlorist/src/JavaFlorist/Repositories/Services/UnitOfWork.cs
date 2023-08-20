using JavaFlorist.Repositories.IServices;

namespace JavaFlorist.Repositories.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IBlogService1 blog1)
        {
            Blogs = blog1;
        }

        public IBlogService1 Blogs { get; }
    }
}
