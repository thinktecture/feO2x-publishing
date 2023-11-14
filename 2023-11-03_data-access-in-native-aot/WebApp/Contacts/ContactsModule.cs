using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Contacts.Common;
using WebApp.Contacts.DeleteContact;
using WebApp.Contacts.GetContact;
using WebApp.Contacts.GetContacts;
using WebApp.Contacts.UpsertContact;

namespace WebApp.Contacts;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(this IServiceCollection services) =>
        services.AddCommonContactsModule()
                .AddGetContactsModule()
                .AddGetContactModule()
                .AddDeleteContactModule()
                .AddUpsertContactModule();
    
    public static WebApplication MapContactEndpoints(this WebApplication app) =>
        app.MapGetContacts()
           .MapGetContact()
           .MapDeleteContact()
           .MapUpsertContact();
}