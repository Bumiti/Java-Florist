using JavaFlorist.Models;
using JavaFlorist.Models.Accounts;
using JavaFlorist.Repositories.IServices;
using JavaFlorist.Repositories.Services;
using JavaFlorist.Repositories.Services.Mail;

namespace JavaFlorist.Configurations
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IAllService<Blog>, BlogService>();
            services.AddTransient<IAllService<Bouquet>, BouquetService>();
            services.AddTransient<IAllService<Category>, CategoryService>();
            services.AddTransient<IAllService<Florist>, FloristService>();
            services.AddTransient<IAllService<OrderDetail>, OrderDetailService>();
            services.AddTransient<IAllService<Order>, OrderService>();
            services.AddTransient<IAllService<Receiver>, ReceiverService>();
            services.AddTransient<IAllService<Status>, StatusService>();
            services.AddTransient<IAllService<User>, UserService>();
            services.AddTransient<IAllService<Voucher>, VoucherService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBlogService1, BlogService1>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFloristAllService, FloristAllService>();
            services.AddHttpContextAccessor();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IBuyerService, BuyerService>();
            services.AddTransient<IDashBoard, DashBoard>();

        }
    }
}
