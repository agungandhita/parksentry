using ParkSentry.Domain.Enums;

namespace ParkSentry.Domain.Entities;

public class Vehicle : BaseEntity
{
    public int OwnerId { get; private set; }
    public string PlateNumber { get; private set; } = string.Empty;
    public string? Brand { get; private set; }
    public string? Model { get; private set; }
    public string? Color { get; private set; }
    public VehicleType VehicleType { get; private set; }
    public int? Year { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties
    public virtual VehicleOwner Owner { get; private set; } = null!;
    public virtual ICollection<ViolationTicket> Tickets { get; private set; } = new List<ViolationTicket>();

    private Vehicle() { }

    public static Vehicle Create(
        int ownerId,
        string plateNumber,
        VehicleType vehicleType,
        string? brand = null,
        string? model = null,
        string? color = null,
        int? year = null)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("Plate number cannot be empty", nameof(plateNumber));

        return new Vehicle
        {
            OwnerId = ownerId,
            PlateNumber = plateNumber,
            VehicleType = vehicleType,
            Brand = brand,
            Model = model,
            Color = color,
            Year = year
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void UpdateDetails(string? brand, string? model, string? color)
    {
        Brand = brand;
        Model = model;
        Color = color;
        MarkAsUpdated();
    }
}
