using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.GetContact;

public interface IGetContactSession : IReadOnlySession
{
    Task<List<GetContactRecord>> GetContactWithAddressesAsync(Guid contactId,
                                                              CancellationToken cancellationToken = default);
}