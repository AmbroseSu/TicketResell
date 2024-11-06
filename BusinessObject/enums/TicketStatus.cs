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

    public static bool IsValidStatus(string status)
    {
        return Enum.TryParse(typeof(TicketStatus), status, true, out _);
    }

}
