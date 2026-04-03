using ParkSentry.Domain.Enums;

namespace ParkSentry.Domain.Entities;

public class Payment : BaseEntity
{
    public int TicketId { get; private set; }
    public string PaymentCode { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public DateTime? PaidAt { get; private set; }
    public string? VerifiedBy { get; private set; }
    public string? Notes { get; private set; }

    // Navigation
    public virtual ViolationTicket Ticket { get; private set; } = null!;

    private Payment() { }

    public static Payment Create(
        int ticketId,
        string paymentCode,
        decimal amount,
        PaymentMethod method,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(paymentCode))
            throw new ArgumentException("Payment code cannot be empty", nameof(paymentCode));

        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero", nameof(amount));

        return new Payment
        {
            TicketId = ticketId,
            PaymentCode = paymentCode,
            Amount = amount,
            Method = method,
            Notes = notes,
            Status = PaymentStatus.Pending
        };
    }

    public void Verify(string verifiedBy)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be verified");

        Status = PaymentStatus.Verified;
        PaidAt = DateTime.UtcNow;
        VerifiedBy = verifiedBy;
        MarkAsUpdated();
    }

    public void Fail()
    {
        Status = PaymentStatus.Failed;
        MarkAsUpdated();
    }

    public void Refund()
    {
        if (Status != PaymentStatus.Verified)
            throw new InvalidOperationException("Only verified payments can be refunded");

        Status = PaymentStatus.Refunded;
        MarkAsUpdated();
    }
}
