using System;
using System.Collections.Generic;
using JavaFlorist.Models;

namespace JavaFlorist.Models
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string Title { get; set; } = null!;
        public string BlogBrief { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual User? User { get; set; }
    }
}
