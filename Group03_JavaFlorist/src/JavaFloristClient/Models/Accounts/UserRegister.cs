using System.ComponentModel.DataAnnotations;

namespace JavaFloristClient.Models.Accounts
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        public string? Address { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public DateTime? Dob { get; set; }
    }
}
