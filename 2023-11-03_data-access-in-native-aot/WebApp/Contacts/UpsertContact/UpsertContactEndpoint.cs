using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApp.CommonValidation;
using WebApp.Contacts.Common;

namespace WebApp.Contacts.UpsertContact;

public static class UpsertContactEndpoint
{
    public static WebApplication MapUpsertContact(this WebApplication app)
    {
        app.MapPut("/api/contacts", UpsertContact);
        return app;
    }

    public static async Task<IResult> UpsertContact(IUpsertContactSession session,
                                                    ContactDetailDtoValidator validator,
                                                    ILoggerFactory loggerFactory,
                                                    ContactDetailDto dto,
                                                    CancellationToken cancellationToken)
    {
        if (validator.CheckForErrors(dto, out var errors))
            return Results.BadRequest(errors);

        await session.UpsertContactAsync(dto, cancellationToken);

        var addressGuids = dto.Addresses.Select(a => a.Id).ToArray();
        var existingAddresses = await session.GetContactAddressesAsync(addressGuids, dto.Id, cancellationToken);
        errors = await UpsertOrDeleteAddressesAsync(dto, existingAddresses, session, cancellationToken);
        if (errors is not null)
            return Results.BadRequest(errors);
        
        await session.SaveChangesAsync(cancellationToken);

        loggerFactory.CreateLogger(typeof(UpsertContactEndpoint))
                     .LogInformation("{@Contact} has been inserted or updated successfully", dto);
        return Results.NoContent();
    }

    private static async Task<Dictionary<string, string[]>?> UpsertOrDeleteAddressesAsync(
        ContactDetailDto contact,
        Dictionary<Guid, Address> existingAddresses,
        IUpsertContactSession session,
        CancellationToken cancellationToken
    )
    {
        Dictionary<string, string[]>? errors = null;
        
        // First, we try to update or insert the addresses that we found in the database
        for (var i = 0; i < contact.Addresses.Length; i++)
        {
            var address = contact.Addresses[i];
            if (!existingAddresses.TryGetValue(address.Id, out var existingAddress))
            {
                var newAddress = new Address(address.Id, contact.Id, address.Street, address.ZipCode, address.City);
                await session.UpsertAddressAsync(newAddress, cancellationToken);
                continue;
            }
            
            // We remove the found address from the dictionary here so that we can delete
            // all the remaining addresses once this loop is finished
            existingAddresses.Remove(address.Id);
            
            // It might be that the passed-in DTO contains addresses with IDs that are already taken.
            // If these addresses do not belong to the current contact, we will flag this as an error.
            if (existingAddress.ContactId != contact.Id)
            {
                errors ??= new ();
                errors.Add(
                    $"addresses[{i}].id",
                    new[] { "There already is an address for another contact with the same ID" }
                );
                continue;
            }

            var updatedAddress = existingAddress with
            {
                Street = address.Street, ZipCode = address.ZipCode, City = address.City
            };
            await session.UpsertAddressAsync(updatedAddress, cancellationToken);
        }

        // If there are any entries left in the dictionary, they will be removed here.
        // They were not part of the DTO coming in and can be safely removed.
        foreach (var (key, _) in existingAddresses)
        {
            await session.RemoveAddressAsync(key, cancellationToken);
        }

        return errors;
    }
}