using JavaFlorist.Models.Accounts;

namespace JavaFlorist.Models
{
    public partial class Status
    {
        public Status()
        {
            Blogs = new HashSet<Blog>();
            Florists = new HashSet<Florist>();
            Orders = new HashSet<Order>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Type { get; set; }
        public int? EntityId { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Florist> Florists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
