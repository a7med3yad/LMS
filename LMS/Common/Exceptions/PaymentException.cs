namespace LMS.Common.Exceptions;

public class PaymentException : Exception
{
    public string? PaymentIntentId { get; }

    public PaymentException() : base() { }
    public PaymentException(string message) : base(message) { }
    public PaymentException(string message, string paymentIntentId) : base(message)
    {
        PaymentIntentId = paymentIntentId;
    }
    public PaymentException(string message, Exception innerException) : base(message, innerException) { }
}
