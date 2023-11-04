using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace WebApp.DatabaseAccess;

public static class DatabaseAccessModule
{
    public static IServiceCollection AddDatabaseAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidDataException("Could not find default connection string in app settings");

        var npgsqlDataSource = NpgsqlDataSource.Create(connectionString);
        return services.AddSingleton(npgsqlDataSource)
                       .AddScoped(sp => sp.GetRequiredService<NpgsqlDataSource>().CreateConnection());
    }
}