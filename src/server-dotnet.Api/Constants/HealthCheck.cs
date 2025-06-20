namespace server_dotnet.Constants;

public static class HealthCheck
{
    public static class Endpoints
    {
        public const string AllDetails = "/health/details";
        public const string LivenessProbe = "/health";
        public const string ReadinessProbe = "/readiness";
    }

    public static class Tags
    {
        public const string Ready = "ready";
        public const string Services = "services";
    }
}
