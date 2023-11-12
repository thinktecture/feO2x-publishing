using Npgsql;

namespace WebApp.DatabaseAccess;

public static class Extensions
{
    public static T? GetOptional<T>(this NpgsqlDataReader reader, int ordinal)
        where T : class
    {
        return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<T>(ordinal);
    }
}