using Microsoft.Extensions.DependencyInjection;
using WebApp.Contacts.Common;

namespace WebApp.Contacts.GetContact;

public static class GetContactModule
{
    public static IServiceCollection AddGetContactModule(this IServiceCollection services) =>
        services.AddScoped<IGetContactSession, NpgsqlGetContactSession>();
}