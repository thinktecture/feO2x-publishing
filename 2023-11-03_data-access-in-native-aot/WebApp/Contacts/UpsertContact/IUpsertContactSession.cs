using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Contacts.Common;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.UpsertContact;

public interface IUpsertContactSession : ISession
{
    Task<Dictionary<Guid, Address>> GetContactAddressesAsync(Guid[] addressIds,
                                                                Guid contactId,
                                                                CancellationToken cancellationToken = default);

    Task UpsertContactAsync(ContactDetailDto dto, CancellationToken cancellationToken = default);
    Task UpsertAddressAsync(Address address, CancellationToken cancellationToken = default);
    Task RemoveAddressAsync(Guid addressId, CancellationToken cancellationToken = default);
}