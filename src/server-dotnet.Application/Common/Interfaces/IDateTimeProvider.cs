namespace server_dotnet.Application.Common.Interfaces;
public interface IDateTimeProvider
{
    public DateTime DateTimeUtcNow { get; }
}
