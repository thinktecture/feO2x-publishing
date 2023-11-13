using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Contacts.DeleteContact;
using WebApp.Contacts.GetContact;
using WebApp.Contacts.GetContacts;

namespace WebApp.Contacts;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(this IServiceCollection services) =>
        services.AddGetContactsModule()
                .AddGetContactModule()
                .AddDeleteContactModule();
    
    public static WebApplication MapContactEndpoints(this WebApplication app) =>
        app.MapGetContacts()
           .MapGetContact()
           .MapDeleteContact();
}