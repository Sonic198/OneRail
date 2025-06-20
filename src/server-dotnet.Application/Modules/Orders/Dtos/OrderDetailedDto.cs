using server_dotnet.Application.Modules.Organizations.Dtos;
using server_dotnet.Application.Modules.Users.Dtos;

namespace server_dotnet.Application.Modules.Orders.Dtos;

public record OrderDetailedDto
{
    public int Id { get; init; }
    public DateTime OrderDate { get; init; }
    public decimal TotalAmount { get; init; }
    public UserDto User { get; init; }
    public OrganizationDto Organization { get; init; }
}