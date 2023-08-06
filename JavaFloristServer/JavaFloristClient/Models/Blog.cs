using JavaFloristClient.Models.Accounts;

namespace JavaFloristClient.Models
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
    }
}
