namespace server_dotnet.Application.Modules.Orders.Dtos;

public record OrderDto
{
    public int Id { get; init; }
    public DateTime OrderDate { get; init; }
    public decimal TotalAmount { get; init; }
    public int UserId { get; init; }
    public int OrganizationId { get; init; }
}
