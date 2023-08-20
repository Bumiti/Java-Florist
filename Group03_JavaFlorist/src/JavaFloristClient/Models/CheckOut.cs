using System.ComponentModel.DataAnnotations;

namespace JavaFloristClient.Models
{
    public class CheckOut
    {
        //Order
        public int Id { get; set; }
        [Required(ErrorMessage = "Sender's full name is required.")]
        //public int? UserId { get; set; }
        public string? SenderFullName { get; set; }
        [Required(ErrorMessage = "Sender's email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? SenderEmail { get; set; }
        [Required(ErrorMessage = "Sender's phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? SenderPhone { get; set; }
        [Required(ErrorMessage = "Sender's address is required.")]
        public string? SenderAddress { get; set; }
        [Required(ErrorMessage = "Order date is required.")]
        public DateTime? OrderDate { get; set; }
        [Required(ErrorMessage = "Receiver's name is required.")]
        //public int? ReceiverId { get; set; }
        public string? ReciverName { get; set; }
        [Required(ErrorMessage = "Receiver's address is required.")]
        public string? ReciverAddress { get; set; }
        [Required(ErrorMessage = "Receiver's phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? ReciverPhone { get; set; }
        [Required(ErrorMessage = "Receiver's date is required.")]
        public DateTime? ReceiverDate { get; set; }
        [Required]
        //public int? FloristId { get; set; }
        //public int? FloristName { get; set; }//id
        //public string? BouquetId { get; set; }
        public List<Cart>? CartItems { get; set; }
        [Required(ErrorMessage = "Message is required.")]
        //public string? MessId { get; set; }
        public int? Messages { get; set; }//id
        public string? Code { get; set; }
        public double? Amount { get; set; }
        //public int? StatusId { get; set; }
        public string? Status { get; set; }
        public bool ShipToMyAddress { get; set; }

    }
}
