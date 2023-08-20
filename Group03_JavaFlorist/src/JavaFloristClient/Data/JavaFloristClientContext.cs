using Microsoft.EntityFrameworkCore;

namespace JavaFloristClient.Data
{
    public class JavaFloristClientContext : DbContext
    {
        public JavaFloristClientContext(DbContextOptions<JavaFloristClientContext> options)
            : base(options)
        {
        }

        public DbSet<JavaFloristClient.Models.Category> Category { get; set; } = default!;

        public DbSet<JavaFloristClient.Models.Receiver>? Receiver { get; set; }

        public DbSet<JavaFloristClient.Models.Status>? Status { get; set; }

        public DbSet<JavaFloristClient.Models.Voucher>? Voucher { get; set; }

        public DbSet<JavaFloristClient.Models.Blog>? Blog { get; set; }

        public DbSet<JavaFloristClient.Models.Bouquet>? Bouquet { get; set; }

        public DbSet<JavaFloristClient.Models.Florist>? Florist { get; set; }

        public DbSet<JavaFloristClient.Models.OrderDetail>? OrderDetail { get; set; }

        public DbSet<JavaFloristClient.Models.Order>? Order { get; set; }

        public DbSet<JavaFloristClient.Models.Accounts.User>? User { get; set; }

        public DbSet<JavaFloristClient.Models.Cart>? Cart { get; set; }

        public DbSet<JavaFloristClient.Models.CheckOut>? CheckOut { get; set; }

        public DbSet<JavaFloristClient.Models.Occasion>? Occasion { get; set; }
    }
}
