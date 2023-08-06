namespace JavaFlorist.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int? ReceiverId { get; set; }
        public string? Address { get; set; }
        public int? FloristId { get; set; }
        public string? BouquetBrief { get; set; }
        public string? Messages { get; set; }
        public double? Amount { get; set; }
        public int? StatusId { get; set; }

        public virtual Receiver? Receiver { get; set; }
        public virtual Status? Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
