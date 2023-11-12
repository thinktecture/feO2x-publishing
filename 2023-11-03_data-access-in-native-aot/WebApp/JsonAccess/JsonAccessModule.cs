using Microsoft.Extensions.DependencyInjection;

namespace WebApp.JsonAccess;

public static class JsonAccessModule
{
    public static IServiceCollection AddJsonAccess(this IServiceCollection services) =>
        services.ConfigureHttpJsonOptions(
            options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, JsonContext.Default)
        );
}