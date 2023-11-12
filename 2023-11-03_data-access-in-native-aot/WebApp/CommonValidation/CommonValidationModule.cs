using Microsoft.Extensions.DependencyInjection;

namespace WebApp.CommonValidation;

public static class CommonValidationModule
{
    public static IServiceCollection AddCommonValidation(this IServiceCollection services) =>
        services.AddSingleton(new GuidValidator());
}