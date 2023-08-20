using System;
using System.Collections.Generic;

namespace JavaFlorist.Models
{
    public partial class Occasion
    {
        public Occasion()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string Type { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
