using JavaFlorist.Models.Accounts;

namespace JavaFlorist.Models
{
    public partial class Florist
    {
        public Florist()
        {
            Bouquets = new HashSet<Bouquet>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Bouquet> Bouquets { get; set; }
    }
}
