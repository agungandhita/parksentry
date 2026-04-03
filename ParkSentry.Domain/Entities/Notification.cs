using ParkSentry.Domain.Enums;

namespace ParkSentry.Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; } = false;
    public int? RelatedTicketId { get; private set; }

    // Navigation
    public virtual User User { get; private set; } = null!;

    private Notification() { }

    public static Notification Create(
        int userId,
        NotificationType type,
        string title,
        string message,
        int? relatedTicketId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty", nameof(message));

        return new Notification
        {
            UserId = userId,
            Type = type,
            Title = title,
            Message = message,
            RelatedTicketId = relatedTicketId
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
        MarkAsUpdated();
    }
}
