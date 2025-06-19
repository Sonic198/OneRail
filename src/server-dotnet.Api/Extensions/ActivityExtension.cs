using System.Diagnostics;

namespace server_dotnet.Extensions;

public static class ActivityExtension
{
    public static string GetCurrentTraceIdentifier(this Activity activity) => activity?.Id ?? string.Empty;
}