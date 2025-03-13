using Domaincrafters.Application;
using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Application.Contracts.Ports;
using Simplifyme.Taskly.Domain.User;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config.Vendors;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Queries;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Repositories;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.UnitOfWork;

namespace Simplifyme.Taskly.Main.Modules.Persistence;

public static class EntityFrameworkServices
{
    public static IServiceCollection AddEntityFrameworkServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddQueries()
            .AddScoped<IUnitOfWork, EntityFrameworkUoW>();

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        return services
            .AddScoped<IUserRepository, UserRepository>();
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        return services
            .AddScoped<IAllUsersQuery, AllUsersQuery>();
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DomainContextBase>();

        context.Database.Migrate();
        Console.WriteLine("Database migrated.");

        return app;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string databaseProvider = configuration.GetValue<string>("Database:Provider")!;
        switch (databaseProvider)
        {
            case "PostgreSQL":
                services.AddDbContext<DomainContextBase, DomainContextPostgres>();
                services.AddDbContext<QueryContextBase, QueryContextPostgres>();
                break;
            default:
                throw new NotSupportedException($"Database provider '{databaseProvider}' is not supported.");
        }

        return services;
    }
}