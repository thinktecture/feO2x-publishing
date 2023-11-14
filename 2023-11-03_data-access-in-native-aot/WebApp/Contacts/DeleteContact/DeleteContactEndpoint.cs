using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApp.CommonValidation;
using WebApp.Contacts.Common;

namespace WebApp.Contacts.DeleteContact;

public static class DeleteContactEndpoint
{
    public static WebApplication MapDeleteContact(this WebApplication app)
    {
        app.MapDelete("/api/contacts/{id:required}", DeleteContact);
        return app;
    }

    public static async Task<IResult> DeleteContact(IDeleteContactSession session,
                                                    GuidValidator validator,
                                                    ILoggerFactory loggerFactory,
                                                    Guid id,
                                                    CancellationToken cancellationToken)
    {
        if (validator.CheckForErrors(new GuidDto(id), out var errors))
            return Results.BadRequest(errors);

        var contactDto = await session.GetContactDetailDtoAsync(id, cancellationToken);
        if (contactDto is null)
            return Results.NotFound();

        await session.DeleteContactAsync(id, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);

        loggerFactory
           .CreateLogger(typeof(DeleteContactEndpoint))
           .LogInformation("{@Contact} was deleted successfully", contactDto);
        return Results.Ok(contactDto);
    }
}