namespace server_dotnet.Application.Common.Responses.Dtos;

public record PropertyValidationDto
{
    public PropertyValidationDto()
    {
    }

    public PropertyValidationDto(string propertyName, params string[] errors)
    {
        PropertyName = propertyName;
        Errors = errors;
    }

    public string PropertyName { get; init; }

    public IEnumerable<string> Errors { get; init; }
}
