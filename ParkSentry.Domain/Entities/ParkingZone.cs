namespace ParkSentry.Domain.Entities;

public class ParkingZone : BaseEntity
{
    public string ZoneCode { get; private set; } = string.Empty;
    public string ZoneName { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? City { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation
    public virtual ICollection<ViolationTicket> Tickets { get; private set; } = new List<ViolationTicket>();

    private ParkingZone() { }

    public static ParkingZone Create(string zoneCode, string zoneName, string? address = null, string? city = null)
    {
        if (string.IsNullOrWhiteSpace(zoneCode))
            throw new ArgumentException("Zone code cannot be empty", nameof(zoneCode));

        return new ParkingZone
        {
            ZoneCode = zoneCode,
            ZoneName = zoneName,
            Address = address,
            City = city
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }
}
