namespace JavaFlorist.Models.Payments
{
    public record StripeCustomer(
        string Name,
        string Email,
        string CustomerId);
}
