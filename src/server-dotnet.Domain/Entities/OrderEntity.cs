namespace server_dotnet.Domain.Entities;

public class OrderEntity
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public required int UserId { get; set; }
    public required int OrganizationId { get; set; }

    public required UserEntity User { get; set; }
    public required OrganizationEntity Organization { get; set; }
}