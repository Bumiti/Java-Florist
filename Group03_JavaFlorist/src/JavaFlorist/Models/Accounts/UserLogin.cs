using System.ComponentModel.DataAnnotations;

namespace JavaFlorist.Models.Accounts
{
    public class UserLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
