using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_dotnet.Domain.Entities;

namespace server_dotnet.Infrastructure.Data.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(t => t.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.LastName)
            .HasMaxLength(200)
            .IsRequired();
    }
}
