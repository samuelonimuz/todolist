using Microsoft.OpenApi.Models;
using Simplifyme.Taskly.Infrastructure.WebApi;
using Simplifyme.Taskly.Infrastructure.WebApi.Shared.Extensions;

namespace Simplifyme.Taskly.Main.Modules.WebApi;

public static class WebApiModule
{
    public static IServiceCollection AddWebApiModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services
            .AddProblemDetails()
            .AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithExposedHeaders("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            })
            .AddOpenApiInfo(
                BuildOpenApiInfo(configuration),
                [configuration["WebApi:ApplicationUrl"]]
            )
            .AddUseCases();
    }

    public static WebApplication UseWebApiModule(this WebApplication app)
    {
        // Swagger is handy during development, but we don't want it in production
        if (!app.Environment.IsProduction())
            app.UseSwagger().UseSwaggerUI();

        app
            .UseCors()
            .UseHttpsRedirection();

        return app.MapRoutes();
    }

    private static OpenApiInfo BuildOpenApiInfo(
        this IConfiguration configuration
    )
    {
        return new OpenApiInfo
        {
            Version = configuration["WebApi:Version"],
            Title = configuration["WebApi:Title"],
            Description = configuration["WebApi:Description"],
            Contact = new OpenApiContact
            {
                Name = configuration["WebApi:Contact:Name"],
                Email = configuration["WebApi:Contact:Email"]
            },
        };
    }
}