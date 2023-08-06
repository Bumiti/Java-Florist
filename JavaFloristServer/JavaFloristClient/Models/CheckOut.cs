namespace JavaFlorist.Models
{
    public class CheckOut
    {
        //Order
        public int Id { get; set; }
        //public int? UserId { get; set; }
        public string? SenderFullName { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderPhone { get; set; }
        public string? SenderAddress { get; set; }
        public DateTime? OrderDate { get; set; }
        //public int? ReceiverId { get; set; }
        public string? ReciverName { get; set; }
        public string? ReciverAddress { get; set; }
        public string? ReciverPhone { get; set; }
        public DateTime? ReceiverDate { get; set; }
        //public int? FloristId { get; set; }
        public string? FloristName { get; set; }
        public string? FloristLogo { get; set; }
        public string? FloristEmail { get; set; }
        public string? FloristPhone { get; set; }
        public string? FloristAddress { get; set; }
        //public string? BouquetId { get; set; }
        public int? BouquetId { get; set; }
        public string? BouquetName { get; set; }
        public double? BouquetPrice { get; set; }
        public int? BouquetQuantity { get; set; }
        public string? BouquetBrief { get; set; }
        //public string? MessId { get; set; }
        public string? Messages { get; set; }
        public double? Amount { get; set; }
        //public int? StatusId { get; set; }
        public string? Status { get; set; }

    }
}
