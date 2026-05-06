namespace LMS.Infrastructure.Services;

public interface IStripeService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, Dictionary<string, string> metadata, CancellationToken ct = default);

    Task<bool> ConfirmPaymentIntentAsync(string paymentIntentId, CancellationToken ct = default);

    Task<string> GetPaymentIntentStatusAsync(string paymentIntentId, CancellationToken ct = default);

    bool ValidateWebhookSignature(string payload, string signature, out object stripeEvent);
}

public class StripeService : IStripeService
{
    public Task<string> CreatePaymentIntentAsync(decimal amount, string currency, Dictionary<string, string> metadata, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<bool> ConfirmPaymentIntentAsync(string paymentIntentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<string> GetPaymentIntentStatusAsync(string paymentIntentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public bool ValidateWebhookSignature(string payload, string signature, out object stripeEvent)
        => throw new NotImplementedException();
}
