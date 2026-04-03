namespace ParkSentry.Domain.Entities;

public class ViolationType : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal FineAmount { get; private set; }

    // Navigation
    public virtual ICollection<ViolationTicket> Tickets { get; private set; } = new List<ViolationTicket>();

    private ViolationType() { }

    public static ViolationType Create(string code, string name, decimal fineAmount, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));

        if (fineAmount < 0)
            throw new ArgumentException("Fine amount cannot be negative", nameof(fineAmount));

        return new ViolationType
        {
            Code = code,
            Name = name,
            FineAmount = fineAmount,
            Description = description
        };
    }

    public void UpdateFineAmount(decimal newAmount)
    {
        if (newAmount < 0)
            throw new ArgumentException("Fine amount cannot be negative", nameof(newAmount));

        FineAmount = newAmount;
        MarkAsUpdated();
    }
}
