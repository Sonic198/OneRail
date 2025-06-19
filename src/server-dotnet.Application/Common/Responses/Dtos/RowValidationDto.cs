namespace server_dotnet.Application.Common.Responses.Dtos;

public record RowValidationDto(int RowIndex, string RowData, IEnumerable<string> Errors);
