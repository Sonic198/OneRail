using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public class ForbiddenResponse : IErrorResponse
{
    public IEnumerable<string> Errors { get; }

    public ForbiddenResponse(IEnumerable<string> errors)
    {
        Errors = errors ?? Enumerable.Empty<string>();
    }
}
