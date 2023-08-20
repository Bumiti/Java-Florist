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
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;

        public virtual ICollection<Bouquet> Bouquets { get; set; }
    }
}
