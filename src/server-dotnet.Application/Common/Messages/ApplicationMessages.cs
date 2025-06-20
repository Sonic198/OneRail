namespace server_dotnet.Application.Common.Messages;

internal static class ApplicationMessages
{
    public static class DateTime
    {
        public const string MustBeInPast = "Date values must occur before the current timestamp.";
    }

    public static class User 
    { 
        public const string NotFound = "User not found.";        
    }

    public static class Organization
    {
        public const string NotFound = "Organization not found.";
    }

    public static class Order
    {
        public const string NotFound = "Order not found.";
    }
}
