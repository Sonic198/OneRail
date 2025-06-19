namespace server_dotnet.Domain.Entities;

public class OrganizationEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Industry { get; set; }
    public DateTime DateFounded { get; set; }

    public IList<UserEntity> Users { get; set; } = new List<UserEntity>();
    public IList<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}
