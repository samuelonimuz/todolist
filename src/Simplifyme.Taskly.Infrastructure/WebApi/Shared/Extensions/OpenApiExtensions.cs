namespace Simplifyme.Taskly.Infrastructure.WebApi.Shared.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiInfo(
        this IServiceCollection services,
        OpenApiInfo openApiInfo,
        List<string> serverUrls
    )
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc(openApiInfo.Version, openApiInfo);
                serverUrls?.ForEach(serverUrl => options.AddServer(new OpenApiServer { Url = serverUrl }));
            });
    }
}
