using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public record OperationFailedResponse(string Message, IEnumerable<object> Errors = default) : IErrorResponse;