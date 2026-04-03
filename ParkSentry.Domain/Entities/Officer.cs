namespace ParkSentry.Domain.Entities;

public class Officer : BaseEntity
{
    public int UserId { get; private set; }
    public string BadgeNo { get; private set; } = string.Empty;
    public string? Department { get; private set; }
    public string? Zone { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual ICollection<ViolationTicket> IssuedTickets { get; private set; } = new List<ViolationTicket>();

    private Officer() { }

    public static Officer Create(int userId, string badgeNo, string? department = null, string? zone = null)
    {
        if (string.IsNullOrWhiteSpace(badgeNo))
            throw new ArgumentException("Badge number cannot be empty", nameof(badgeNo));

        return new Officer
        {
            UserId = userId,
            BadgeNo = badgeNo,
            Department = department,
            Zone = zone
        };
    }

    public void UpdateZone(string zone)
    {
        Zone = zone;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }
}
