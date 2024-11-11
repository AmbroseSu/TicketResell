namespace BusinessObject.Enums;

public enum TicketStatus
{
    PENDING,
    ACTIVE,
    CLOSED
}

public static class StatusExtensions
{
    public static bool IsValidEnum<TEnum>(TEnum status) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), status);
    }
}
