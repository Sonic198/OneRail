using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<OrderEntity> Orders { get; }
    DbSet<OrganizationEntity> Organizations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
