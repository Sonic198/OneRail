namespace server_dotnet.Constants;

public static class HealthCheck
{
    public static class Endpoints
    {
        public const string AllDetails = "/health";
        public const string LivenessProbe = "/health/live";
        public const string ReadinessProbe = "/health/ready";
    }

    public static class Tags
    {
        public const string Ready = "ready";
        public const string Services = "services";
    }
}
