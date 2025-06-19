using System.Diagnostics;
using server_dotnet.Services.Interfaces;

namespace server_dotnet.Services;

internal class ActivityProvider : IActivityProvider
{
    public Activity Current => Activity.Current;
}
