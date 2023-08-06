using System;
using System.Collections.Generic;

namespace JavaFlorist.Models
{
    public partial class Category
    {
        public Category()
        {
            Bouquets = new HashSet<Bouquet>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }

        public virtual ICollection<Bouquet> Bouquets { get; set; }
    }
}
