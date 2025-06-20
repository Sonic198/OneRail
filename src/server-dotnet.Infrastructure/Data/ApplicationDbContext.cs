using System.Reflection;
using Microsoft.EntityFrameworkCore;
using server_dotnet.Application.Common.Interfaces;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Infrastructure.Data;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    public DbSet<OrganizationEntity> Organizations => Set<OrganizationEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
