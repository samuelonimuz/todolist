using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Application.Contracts.Data;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Data;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

public abstract class QueryContextBase : DbContext
{
    public DbSet<UserData> Users { get; set; }

    protected QueryContextBase()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDataConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
