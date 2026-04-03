using ParkSentry.Domain.Enums;

namespace ParkSentry.Domain.Entities;

public class ViolationTicket : BaseEntity
{
    public string TicketNo { get; private set; } = string.Empty;
    public int OfficerId { get; private set; }
    public int VehicleId { get; private set; }
    public int ViolationTypeId { get; private set; }
    public int ZoneId { get; private set; } // NOT NULL sesuai schema SQL
    public DateTime ViolationDate { get; private set; }
    public decimal FineAmount { get; private set; }
    public DateOnly DueDate { get; private set; }
    public string? LocationDetail { get; private set; } // sesuai kolom location_detail
    public string? PhotoEvidence { get; private set; }
    public string? Notes { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Unpaid;

    // Navigation properties
    public virtual Officer Officer { get; private set; } = null!;
    public virtual Vehicle Vehicle { get; private set; } = null!;
    public virtual ViolationType ViolationType { get; private set; } = null!;
    public virtual ParkingZone Zone { get; private set; } = null!;

    private ViolationTicket() { }

    public static ViolationTicket Create(
        string ticketNo,
        int officerId,
        int vehicleId,
        int violationTypeId,
        int zoneId,
        decimal fineAmount,
        DateTime violationDate,
        string? locationDetail = null,
        string? photoEvidence = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(ticketNo))
            throw new ArgumentException("Ticket number cannot be empty", nameof(ticketNo));

        return new ViolationTicket
        {
            TicketNo = ticketNo,
            OfficerId = officerId,
            VehicleId = vehicleId,
            ViolationTypeId = violationTypeId,
            ZoneId = zoneId,
            FineAmount = fineAmount,
            ViolationDate = violationDate,
            DueDate = DateOnly.FromDateTime(violationDate.AddDays(14)),
            LocationDetail = locationDetail,
            PhotoEvidence = photoEvidence,
            Notes = notes,
            Status = TicketStatus.Unpaid
        };
    }

    // Business Logic
    public void MarkAsPaid()
    {
        if (Status != TicketStatus.Unpaid)
            throw new InvalidOperationException($"Ticket cannot be paid. Current status: {Status}");

        Status = TicketStatus.Paid;
        MarkAsUpdated();
    }

    public void MarkAsAppealing()
    {
        if (Status != TicketStatus.Unpaid)
            throw new InvalidOperationException("Only unpaid tickets can be appealed");

        Status = TicketStatus.Appealing;
        MarkAsUpdated();
    }

    public void Cancel()
    {
        Status = TicketStatus.Cancelled;
        MarkAsUpdated();
    }

    public void Waive()
    {
        Status = TicketStatus.Waived;
        MarkAsUpdated();
    }

    public bool IsOverdue() => Status == TicketStatus.Unpaid && DateTime.UtcNow > ViolationDate.Add(DueDate.ToDateTime(TimeOnly.MinValue) - DateTime.MinValue);
}
