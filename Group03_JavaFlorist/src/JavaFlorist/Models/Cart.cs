namespace JavaFlorist.Models
{
    public class Cart
    {
        public int? Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int? FloristId { get; set; }
    }
}
