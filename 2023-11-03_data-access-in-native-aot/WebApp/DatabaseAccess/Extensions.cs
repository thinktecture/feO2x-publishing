using Npgsql;

namespace WebApp.DatabaseAccess;

public static class Extensions
{
    public static T? GetOptional<T>(this NpgsqlDataReader reader, int ordinal)
        where T : class
    {
        return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<T>(ordinal);
    }
    
    public static void AddBatchCommand<T>(this NpgsqlBatch batch, string sql, T parameter)
    {
        var batchCommand = new NpgsqlBatchCommand(sql);
        batchCommand.Parameters.Add(new NpgsqlParameter<T> { TypedValue = parameter });
        batch.BatchCommands.Add(batchCommand);
    }

    public static void AddBatchCommand<T1, T2, T3, T4, T5>(this NpgsqlBatch batch,
                                                           string sql,
                                                           T1 firstParameter,
                                                           T2 secondParameter,
                                                           T3 thirdParameter,
                                                           T4 fourthParameter,
                                                           T5 fifthParameter)
    {
        var batchCommand = new NpgsqlBatchCommand(sql);
        batchCommand.Parameters.Add(new NpgsqlParameter<T1> { TypedValue = firstParameter });
        batchCommand.Parameters.Add(new NpgsqlParameter<T2> { TypedValue = secondParameter });
        batchCommand.Parameters.Add(new NpgsqlParameter<T3> { TypedValue = thirdParameter });
        batchCommand.Parameters.Add(new NpgsqlParameter<T4> { TypedValue = fourthParameter });
        batchCommand.Parameters.Add(new NpgsqlParameter<T5> { TypedValue = fifthParameter });
        batch.BatchCommands.Add(batchCommand);
    }
}