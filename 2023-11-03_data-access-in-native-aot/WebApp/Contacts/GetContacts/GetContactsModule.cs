using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Contacts.GetContacts;

public static class GetContactsModule
{
    public static IServiceCollection AddGetContactsModule(this IServiceCollection services) =>
        services.AddScoped<IGetContactsSession, NpgsqlGetContactsSession>()
                .AddSingleton(new PagingParametersValidator());
}