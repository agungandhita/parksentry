namespace ParkSentry.Domain.Entities;

/// <summary>
/// Base Entity menggunakan OOP Inheritance
/// Semua entity akan inherit dari class ini
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// Method untuk update timestamp (Encapsulation)
    /// Protected agar hanya bisa dipanggil dari child class
    /// </summary>
    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
