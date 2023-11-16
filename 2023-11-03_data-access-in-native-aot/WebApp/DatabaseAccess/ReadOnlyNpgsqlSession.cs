using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace WebApp.DatabaseAccess;

public abstract class ReadOnlyNpgsqlSession(
        NpgsqlConnection connection,
        IsolationLevel? transactionLevel = null
    )
    : IReadOnlySession
{
    protected NpgsqlConnection Connection => connection;
    protected IsolationLevel? TransactionLevel => transactionLevel;
    protected NpgsqlTransaction? Transaction { get; private set; }

    private bool IsInitialized
    {
        get
        {
            if (transactionLevel.HasValue && Transaction is null)
                return false;
            return connection.State is ConnectionState.Open;
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (Transaction is not null)
            await Transaction.DisposeAsync();
        await connection.DisposeAsync();
    }

    public virtual void Dispose()
    {
        Transaction?.Dispose();
        connection.Dispose();
    }

    public ValueTask<NpgsqlCommand> CreateCommandAsync(string? sql = null,
                                                       CancellationToken cancellationToken = default)
    {
        return IsInitialized ? new (CreateCommand(sql)) : InitializeAndCreateCommandAsync(sql, cancellationToken);
    }

    public ValueTask<NpgsqlBatch> CreateBatchAsync(CancellationToken cancellationToken = default)
    {
        return IsInitialized ? new (CreateBatch()) : InitializeAndCreateBatchAsync(cancellationToken);
    }

    private async ValueTask<NpgsqlCommand> InitializeAndCreateCommandAsync(string? sql,
                                                                           CancellationToken cancellationToken)
    {
        await InitializeAsync(cancellationToken);
        return CreateCommand(sql);
    }

    private async ValueTask<NpgsqlBatch> InitializeAndCreateBatchAsync(CancellationToken cancellationToken)
    {
        await InitializeAsync(cancellationToken);
        return CreateBatch();
    }

    private async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        if (transactionLevel.HasValue && Transaction is null)
            Transaction = await connection.BeginTransactionAsync(cancellationToken);
    }

    private NpgsqlCommand CreateCommand(string? sql)
    {
        var command = connection.CreateCommand();
        command.Transaction = Transaction;
        if (sql is not null)
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
        }

        return command;
    }

    private NpgsqlBatch CreateBatch()
    {
        var batch = connection.CreateBatch();
        batch.Transaction = Transaction;
        return batch;
    }
}