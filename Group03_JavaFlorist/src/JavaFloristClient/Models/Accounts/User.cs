using System;
using System.Collections.Generic;

namespace JavaFloristClient.Models.Accounts
{
    public partial class User
    {
        public User()
        {
            Blogs = new HashSet<Blog>();
            Florists = new HashSet<Florist>();
            Vouchers = new HashSet<Voucher>();
        }

        public int Id { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public string Firstname { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public string Role { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
        public int? StatusId { get; set; }

        public virtual Status? Status { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Florist> Florists { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
