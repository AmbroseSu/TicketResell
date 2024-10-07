namespace BusinessObject.Enums;

public enum TicketStatus
{
    PENDING,
    VERIFIED,
    REJECTED,
    CLOSED
}

public static class TicketStatusExtensions
{
    public static string ToFriendlyString(this TicketStatus status)
    {
        return status.ToString().ToLowerInvariant() switch
        {
            "pending" => "PENDING",
            "verified" => "VERIFIED",
            "rejected" => "REJECTED",
            "closed" => "CLOSED",
            _ => status.ToString()
        };
    }
}
