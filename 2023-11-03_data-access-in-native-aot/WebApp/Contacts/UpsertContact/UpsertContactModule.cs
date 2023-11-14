using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Contacts.UpsertContact;

public static class UpsertContactModule
{
    public static IServiceCollection AddUpsertContactModule(this IServiceCollection services) =>
        services.AddScoped<IUpsertContactSession, NpgsqlUpsertContactSession>();
}