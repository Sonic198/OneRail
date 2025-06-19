namespace server_dotnet.Application.Modules.Users.Dtos;

public record UserDto
{
    public int Id { get; init; }
    public string? First { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public int OrganizationId { get; init; }
    public DateTime DateCreated { get; init; }
}
