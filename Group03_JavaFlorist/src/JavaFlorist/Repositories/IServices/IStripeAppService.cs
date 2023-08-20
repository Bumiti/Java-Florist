using JavaFlorist.Models.Payments;

namespace JavaFlorist.Repositories.IServices
{
    public interface IStripeAppService
    {
        
            Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
            Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
        
    }
}


