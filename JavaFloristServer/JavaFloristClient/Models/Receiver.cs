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
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime? ReceiverDate { get; set; }
        public string? Message { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
