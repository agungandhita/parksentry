namespace ParkSentry.Domain.Entities;

public class VehicleOwner : BaseEntity
{
    public int UserId { get; private set; }
    public string IdCardNo { get; private set; } = string.Empty;
    public string? Address { get; private set; }

    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual ICollection<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
    public virtual ICollection<Appeal> Appeals { get; private set; } = new List<Appeal>();

    private VehicleOwner() { }

    public static VehicleOwner Create(int userId, string idCardNo, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(idCardNo))
            throw new ArgumentException("ID card number cannot be empty", nameof(idCardNo));

        return new VehicleOwner
        {
            UserId = userId,
            IdCardNo = idCardNo,
            Address = address
        };
    }

    public void UpdateAddress(string address)
    {
        Address = address;
        MarkAsUpdated();
    }
}
