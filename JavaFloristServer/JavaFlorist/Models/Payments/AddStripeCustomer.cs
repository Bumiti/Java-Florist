namespace JavaFlorist.Models.Payments
{
    public record AddStripeCustomer(
        string Email,
        string Name,
        AddStripeCard CreditCard);
}
