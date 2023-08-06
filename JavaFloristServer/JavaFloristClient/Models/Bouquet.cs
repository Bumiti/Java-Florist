using System;
using System.Collections.Generic;

namespace JavaFloristClient.Models
{
    public partial class Bouquet
    {
        public Bouquet()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UnitBrief { get; set; }
        public double? UnitPrice { get; set; }
        public string? Image { get; set; }
        public DateTime? BouquetDate { get; set; }
        public bool? Available { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? FloristId { get; set; }
        public int? Quantity { get; set; }
        public double? Discount { get; set; }
        public string? Tag { get; set; }
        public double? PriceAfter { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Florist? Florist { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
