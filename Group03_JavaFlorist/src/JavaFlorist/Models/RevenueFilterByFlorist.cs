namespace JavaFlorist.Models
{
    public class RevenueFilterByFlorist
    {
        public int? FloristId { get; set; }
        public string FloristName { get; set; }
        public string FloristLogo { get; set; }
        public int? OrderQuantity { get; set; }
        public double? TotalMoney { get; set; }
        public int? BouquetQuantity { get; set; }
        public int? UserQuantity { get; set; }
        public int? FloristQuantity { get; set; }
    }
}
