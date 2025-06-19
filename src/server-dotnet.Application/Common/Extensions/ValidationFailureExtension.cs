using FluentValidation.Results;
using server_dotnet.Application.Common.Responses.Dtos;

namespace server_dotnet.Application.Common.Extensions;

public static class ValidationFailureExtension
{
    public static IEnumerable<PropertyValidationDto> GetErrors(this IEnumerable<ValidationFailure> validationFailures)
        => validationFailures?.GroupBy(n => n.PropertyName)
            .Select(n =>
                new PropertyValidationDto
                {
                    PropertyName = n.Key,
                    Errors = n.Select(m => m.ErrorMessage)
                }) ?? [];
}
