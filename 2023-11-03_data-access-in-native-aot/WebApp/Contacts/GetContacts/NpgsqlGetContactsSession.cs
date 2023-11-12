using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Light.EmbeddedResources;
using Npgsql;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.GetContacts;

public sealed class NpgsqlGetContactsSession(NpgsqlConnection connection)
    : ReadOnlyNpgsqlSession(connection), IGetContactsSession
{
    public async Task<List<ContactListDto>> GetContactsAsync(int skip,
                                                             int take,
                                                             CancellationToken cancellationToken = default)
    {
        await using var command =
            await CreateCommandAsync(this.GetEmbeddedResource("GetContacts.sql"), cancellationToken);
        command.Parameters.Add(new NpgsqlParameter<int> { TypedValue = skip });
        command.Parameters.Add(new NpgsqlParameter<int> { TypedValue = take });
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken);
        return await DeserializeContactsAsync(reader, cancellationToken);
    }

    private async Task<List<ContactListDto>> DeserializeContactsAsync(NpgsqlDataReader reader,
                                                                      CancellationToken cancellationToken)
    {
        var contacts = new List<ContactListDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var id = reader.GetGuid(0);
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);
            var email = reader.GetString(3);
            var phoneNumber = reader.GetString(4);
            contacts.Add(new (id, firstName, lastName, email, phoneNumber));
        }

        return contacts;
    }
}