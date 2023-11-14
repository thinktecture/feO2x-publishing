using Microsoft.Extensions.DependencyInjection;

namespace WebApp.Contacts.Common;

public static class CommonContactsModule
{
    public static IServiceCollection AddCommonContactsModule(this IServiceCollection services) =>
        services.AddSingleton<ContactDetailDtoValidator>()
                .AddSingleton<AddressDtoValidator>();
}