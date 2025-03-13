namespace Simplifyme.Taskly.Main.Modules.Persistence;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule
    (
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services
            .AddEntityFrameworkServices(configuration);
    }

    public static WebApplication UsePersistenceModule(this WebApplication app)
    {
        return app.ApplyMigrations();
    }
}