using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace WebApp.DatabaseAccess;

public abstract class AsyncNpgsqlSession(NpgsqlConnection connection,
                                         IsolationLevel? transactionLevel = IsolationLevel.ReadCommitted)
    : AsyncReadOnlyNpgsqlSession(connection, transactionLevel), IAsyncSession
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction is not null)
            await Transaction.CommitAsync(cancellationToken);
    }
}