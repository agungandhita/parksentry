using ParkSentry.Domain.Enums;

namespace ParkSentry.Domain.Entities;

public class Appeal : BaseEntity
{
    public int TicketId { get; private set; }
    public int OwnerId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string? EvidenceFile { get; private set; }
    public AppealStatus Status { get; private set; } = AppealStatus.Pending;
    public string? ReviewerNotes { get; private set; }
    public DateTime? ReviewedAt { get; private set; }

    // Navigation
    public virtual ViolationTicket Ticket { get; private set; } = null!;
    public virtual VehicleOwner Owner { get; private set; } = null!;

    private Appeal() { }

    public static Appeal Create(int ticketId, int ownerId, string reason, string? evidenceFile = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Appeal reason cannot be empty", nameof(reason));

        return new Appeal
        {
            TicketId = ticketId,
            OwnerId = ownerId,
            Reason = reason,
            EvidenceFile = evidenceFile,
            Status = AppealStatus.Pending
        };
    }

    public void StartReview()
    {
        Status = AppealStatus.Reviewing;
        MarkAsUpdated();
    }

    public void Approve(string? reviewerNotes = null)
    {
        Status = AppealStatus.Approved;
        ReviewerNotes = reviewerNotes;
        ReviewedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void Reject(string? reviewerNotes = null)
    {
        Status = AppealStatus.Rejected;
        ReviewerNotes = reviewerNotes;
        ReviewedAt = DateTime.UtcNow;
        MarkAsUpdated();
    }
}
