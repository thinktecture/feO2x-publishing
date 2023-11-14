using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Light.EmbeddedResources;
using Npgsql;
using NpgsqlTypes;
using WebApp.Contacts.Common;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.UpsertContact;

public sealed class NpgsqlUpsertContactSession(NpgsqlConnection connection)
    : NpgsqlSession(connection), IUpsertContactSession
{
    private static readonly string GetContactAddressesSql =
        typeof(NpgsqlUpsertContactSession).GetEmbeddedResource("GetContactAddresses.sql");

    private static readonly string UpsertContactSql =
        typeof(NpgsqlUpsertContactSession).GetEmbeddedResource("UpsertContact.sql");

    private static readonly string UpsertAddressSql =
        typeof(NpgsqlUpsertContactSession).GetEmbeddedResource("UpsertAddress.sql");

    private static readonly string DeleteAddressSql =
        typeof(NpgsqlUpsertContactSession).GetEmbeddedResource("DeleteAddress.sql");

    private NpgsqlBatch? _npgsqlBatch;

    public async Task<Dictionary<Guid, Address>> GetContactAddressesAsync(
        Guid[] addressIds,
        Guid contactId,
        CancellationToken cancellationToken = default
    )
    {
        await using var command = await CreateCommandAsync(GetContactAddressesSql, cancellationToken);
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags -- see comment on NpgsqlDbType.Array
        command.Parameters.Add(new () { Value = addressIds, NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Uuid });
        command.Parameters.Add(new NpgsqlParameter<Guid> { TypedValue = contactId });
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken);
        return await DeserializeAddressesAsync(reader, cancellationToken);
    }

    public async Task UpsertContactAsync(ContactDetailDto dto, CancellationToken cancellationToken = default)
    {
        var batch = await GetRequiredBatchAsync(cancellationToken);
        batch.AddBatchCommand(UpsertContactSql, dto.Id, dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);
    }

    public async Task UpsertAddressAsync(Address address, CancellationToken cancellationToken = default)
    {
        var batch = await GetRequiredBatchAsync(cancellationToken);
        batch.AddBatchCommand(UpsertAddressSql, address.Id, address.ContactId, address.Street, address.ZipCode, address.City);
    }

    public async Task RemoveAddressAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        var batch = await GetRequiredBatchAsync(cancellationToken);
        batch.AddBatchCommand(DeleteAddressSql, addressId);
    }

    public override async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_npgsqlBatch is null)
            throw new InvalidOperationException("You must call any of the batch methods before calling SaveChangesAsync");

        await _npgsqlBatch.ExecuteNonQueryAsync(cancellationToken);
        await base.SaveChangesAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _npgsqlBatch?.Dispose();
        base.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        if (_npgsqlBatch is not null)
            await _npgsqlBatch.DisposeAsync();
        await base.DisposeAsync();
    }

    private static async Task<Dictionary<Guid, Address>> DeserializeAddressesAsync(NpgsqlDataReader reader,
                                                                                   CancellationToken cancellationToken)
    {
        var addresses = new Dictionary<Guid, Address>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var id = reader.GetGuid(0);
            var contactId = reader.GetGuid(1);
            var street = reader.GetString(2);
            var zipCode = reader.GetString(3);
            var city = reader.GetString(4);
            addresses.Add(id, new (id, contactId, street, zipCode, city));
        }

        return addresses;
    }

    private ValueTask<NpgsqlBatch> GetRequiredBatchAsync(CancellationToken cancellationToken) =>
        _npgsqlBatch is not null ? new (_npgsqlBatch) : InitializeBatchAsync(cancellationToken);

    private async ValueTask<NpgsqlBatch> InitializeBatchAsync(CancellationToken cancellationToken)
    {
        _npgsqlBatch = await CreateBatchAsync(cancellationToken);
        return _npgsqlBatch;
    }
}