using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Contacts.Common;
using WebApp.DatabaseAccess;

namespace WebApp.Contacts.DeleteContact;

public interface IDeleteContactSession : IGetContactSession, ISession
{
    Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);
}