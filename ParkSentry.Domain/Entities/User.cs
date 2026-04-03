namespace ParkSentry.Domain.Entities;

public class User : BaseEntity
{
    public int RoleId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty; // TODO: Hash password
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; } = true;

    // Navigation properties (OOP Relationships)
    public virtual Role Role { get; private set; } = null!;
    public virtual Officer? Officer { get; private set; }
    public virtual VehicleOwner? VehicleOwner { get; private set; }
    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();

    // Private constructor
    private User() { }

    // Factory Method dengan validation
    public static User Create(
        int roleId,
        string fullName,
        string username,
        string password,
        string? email = null,
        string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        return new User
        {
            RoleId = roleId,
            FullName = fullName,
            Username = username,
            Password = password, // TODO: Hash dengan BCrypt
            Email = email,
            Phone = phone
        };
    }

    // Business Logic Methods (OOP Behavior)
    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    public void UpdateProfile(string fullName, string? email, string? phone)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        MarkAsUpdated();
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty", nameof(newPassword));

        Password = newPassword; // TODO: Hash
        MarkAsUpdated();
    }
}
