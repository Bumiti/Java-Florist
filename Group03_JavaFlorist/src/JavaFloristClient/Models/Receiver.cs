using System;
using System.Collections.Generic;

namespace JavaFloristClient.Models
{
    public partial class Receiver
    {
        public Receiver()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime ReceiverDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
