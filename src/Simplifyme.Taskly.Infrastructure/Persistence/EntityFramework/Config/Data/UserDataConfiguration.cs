using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simplifyme.Taskly.Application.Contracts.Data;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Data;

public sealed class UserDataConfiguration : IEntityTypeConfiguration<UserData>
{
    public void Configure(EntityTypeBuilder<UserData> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.UserName);
    }
}