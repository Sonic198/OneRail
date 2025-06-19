namespace server_dotnet.Application.Common.Responses.Interfaces;

public interface IObjectCreateResponse : ISuccessResponse
{
    int Id { get; }
}
