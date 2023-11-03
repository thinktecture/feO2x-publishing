using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace WebApp.DatabaseAccess;

public abstract class AsyncReadOnlyNpgsqlSession(NpgsqlConnection connection, IsolationLevel? transactionLevel = null)
    : IAsyncReadOnlySession
{
    protected NpgsqlTransaction? Transaction { get; private set; }
    private bool IsInitialized => 
        (!transactionLevel.HasValue || Transaction is not null) && connection.State is ConnectionState.Open;

    protected ValueTask<NpgsqlCommand> CreateCommandAsync(string? sql = null,
                                                          CancellationToken cancellationToken = default) =>
        IsInitialized ? new (CreateCommand(sql)) : InitializeAndCreateCommandAsync(sql, cancellationToken);

    private async ValueTask<NpgsqlCommand> InitializeAndCreateCommandAsync(string? sql,
                                                                           CancellationToken cancellationToken)
    {
        await InitializeAsync(cancellationToken);
        return CreateCommand(sql);
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
    
    public async ValueTask DisposeAsync()
    {
        if (Transaction is not null)
            await Transaction.DisposeAsync();
        await connection.DisposeAsync();
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        connection.Dispose();
    }
}