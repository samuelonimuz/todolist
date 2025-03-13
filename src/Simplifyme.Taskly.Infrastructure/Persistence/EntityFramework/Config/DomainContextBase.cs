using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Domain.User;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Domain;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

public abstract class DomainContextBase : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        base.OnModelCreating(modelBuilder);

        modelBuilder.Seed();
    }

}