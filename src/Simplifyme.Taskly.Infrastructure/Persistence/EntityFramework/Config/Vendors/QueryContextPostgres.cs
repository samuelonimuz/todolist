using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Vendors;

public sealed class QueryContextPostgres(
    IConfiguration configuration
) : QueryContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(BuildConnectionString());
    }

    private string BuildConnectionString()
    {
        string connectionString = configuration.GetValue<string>("Database:ConnectionString")!;
        string username = configuration.GetValue<string>("Database:Username")!;
        string password = configuration.GetValue<string>("Database:Password")!;

        return connectionString.Replace("{username}", username).Replace("{password}", password);
    }
}