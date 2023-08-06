using System;
using System.Collections.Generic;
using JavaFlorist.Models.Accounts;

namespace JavaFlorist.Models
{
    public partial class Voucher
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? UserId { get; set; }
        public double? DiscountPercent { get; set; }
        public string? Type { get; set; }

        public virtual User? User { get; set; }
    }
}
