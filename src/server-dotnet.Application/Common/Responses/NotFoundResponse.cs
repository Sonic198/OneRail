using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public record NotFoundResponse(string Message) : IErrorResponse;
