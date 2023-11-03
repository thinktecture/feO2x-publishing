using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DatabaseAccess;

public interface ISession : IReadOnlySession
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}