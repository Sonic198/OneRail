using server_dotnet.Application.Common.Responses.Dtos;
using server_dotnet.Application.Common.Responses.Interfaces;

namespace server_dotnet.Application.Common.Responses;

public class ValidationFailedResponse : IErrorResponse
{
    public IEnumerable<PropertyValidationDto> Errors { get; }

    public ValidationFailedResponse(IEnumerable<PropertyValidationDto> errors)
    {
        Errors = errors ?? Array.Empty<PropertyValidationDto>();
    }

    public ValidationFailedResponse(params PropertyValidationDto[] errors)
    {
        Errors = errors ?? Array.Empty<PropertyValidationDto>();
    }
}
