using System;
using System.Collections.Generic;
using JavaFlorist.Models;

namespace JavaFlorist.Models
{
    public partial class Florist
    {
        public Florist()
        {
            Bouquets = new HashSet<Bouquet>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int? UserId { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Bouquet> Bouquets { get; set; }
    }
}
