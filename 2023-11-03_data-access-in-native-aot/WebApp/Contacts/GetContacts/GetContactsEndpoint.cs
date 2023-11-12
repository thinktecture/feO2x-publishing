using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApp.Validation;

namespace WebApp.Contacts.GetContacts;

public static class GetContactsEndpoint
{
    public static WebApplication MapGetContacts(this WebApplication app)
    {
        app.MapGet("/api/contacts", GetContacts);
        return app;
    }

    public static async Task<IResult> GetContacts(IGetContactsSession session,
                                                  PagingParametersValidator validator,
                                                  CancellationToken cancellationToken,
                                                  int skip = 0,
                                                  int take = 20)
    {
        if (validator.CheckForErrors(new PagingParameters(skip, take), out var errors))
            return Results.BadRequest(errors);

        var contacts = await session.GetContactsAsync(skip, take, cancellationToken);
        return Results.Ok(contacts);
    }
}