using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DatabaseAccess;

public interface IAsyncSession : IAsyncReadOnlySession
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}