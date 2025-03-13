using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Simplifyme.Taskly.Domain.User;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Domain;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => new UserId(value)
        );
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
    }
}