using System;
using System.Collections.Generic;

namespace JavaFloristClient.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? BouquetId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double? Discount { get; set; }

        public virtual Bouquet? Bouquet { get; set; }
        public virtual Order? Order { get; set; }
    }
}
