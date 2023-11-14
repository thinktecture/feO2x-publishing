using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Light.EmbeddedResources;
using Npgsql;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.Common;

public static class GetContactExtensions
{
    public static async Task<List<GetContactRecord>> GetContactAsync(this ReadOnlyNpgsqlSession session,
                                                                     Guid contactId,
                                                                     CancellationToken cancellationToken = default)
    {
        var sql = typeof(GetContactExtensions).GetEmbeddedResource("GetContact.sql");
        await using var command = await session.CreateCommandAsync(sql, cancellationToken);
        command.Parameters.Add(new NpgsqlParameter<Guid> { TypedValue = contactId });
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken);
        return await DeserializeRecordsAsync(reader, cancellationToken);
    }

    private static async Task<List<GetContactRecord>> DeserializeRecordsAsync(NpgsqlDataReader reader,
                                                                              CancellationToken cancellationToken)
    {
        var records = new List<GetContactRecord>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var contactId = reader.GetGuid(0);
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);
            var email = reader.GetOptional<string>(3);
            var phoneNumber = reader.GetOptional<string>(4);
            var addressId = reader.GetFieldValue<Guid?>(5);
            var street = reader.GetOptional<string>(6);
            var zipCode = reader.GetOptional<string>(7);
            var city = reader.GetOptional<string>(8);
            records.Add(new (contactId, firstName, lastName, email, phoneNumber, addressId, street, zipCode, city));
        }

        return records;
    }

    public static async Task<ContactDetailDto?> GetContactDetailDtoAsync(this IGetContactSession session,
                                                                         Guid contactId,
                                                                         CancellationToken cancellationToken = default)
    {
        var records = await session.GetContactWithAddressesAsync(contactId, cancellationToken);
        return records.ConvertToDto();
    }
}