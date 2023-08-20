using System;
using System.Collections.Generic;

namespace JavaFlorist.Models2
{
    public partial class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int? UserId { get; set; }
        public double DiscountPercent { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
    }
}
