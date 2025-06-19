using server_dotnet.Application.Common.Interfaces;

namespace server_dotnet.Infrastructure.Services;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
}
