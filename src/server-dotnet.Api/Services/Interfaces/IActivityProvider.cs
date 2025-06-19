using System.Diagnostics;

namespace server_dotnet.Services.Interfaces;

public interface IActivityProvider
{
    Activity Current { get; }
}
