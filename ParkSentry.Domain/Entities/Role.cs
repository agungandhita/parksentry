namespace ParkSentry.Domain.Entities;

public class Role : BaseEntity
{
    // Properties dengan private set (Encapsulation)
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // Navigation property
    public virtual ICollection<User> Users { get; private set; } = new List<User>();

    // Private constructor untuk Factory Pattern
    private Role() { }

    // Factory Method (OOP Best Practice)
    public static Role Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));

        return new Role
        {
            Name = name,
            Description = description
        };
    }

    // Method untuk update (Behavior - OOP)
    public void UpdateDescription(string description)
    {
        Description = description;
        MarkAsUpdated();
    }
}
