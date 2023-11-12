using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.GetContacts;

public interface IGetContactsSession : IReadOnlySession
{
    Task<List<ContactListDto>> GetContactsAsync(int skip, int take, CancellationToken cancellationToken = default);
}