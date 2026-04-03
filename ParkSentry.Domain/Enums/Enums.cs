namespace ParkSentry.Domain.Enums;

public enum TicketStatus
{
    Unpaid,
    Paid,
    Appealing,
    Cancelled,
    Waived
}

public enum PaymentMethod
{
    Cash,
    Transfer,
    Qris,
    VirtualAccount
}

public enum PaymentStatus
{
    Pending,
    Verified,
    Failed,
    Refunded
}

public enum AppealStatus
{
    Pending,
    Reviewing,
    Approved,
    Rejected
}

public enum VehicleType
{
    Car,
    Motorcycle,
    Truck,
    Bus
}

public enum NotificationType
{
    TicketIssued,
    AppealUpdate,
    PaymentConfirmed,
    DueReminder
}
