namespace server_dotnet.Domain.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public int? OrganizationId { get; set; }
    public DateTime DateCreated { get; set; }

    public OrganizationEntity? Organization { get; set; }
    public List<OrderEntity>? Orders { get; set; }
}
