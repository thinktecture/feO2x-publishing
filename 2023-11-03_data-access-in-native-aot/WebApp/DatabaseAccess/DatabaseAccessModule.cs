using System.IO;
using System.Threading.Tasks;
using Light.EmbeddedResources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace WebApp.DatabaseAccess;

public static class DatabaseAccessModule
{
    public static IServiceCollection AddDatabaseAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidDataException("Could not find default connection string in app settings");

        return
            services
               .AddSingleton<NpgsqlDataSource>(
                    sp => new NpgsqlSlimDataSourceBuilder(connectionString)
                         .UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>())
                         .Build()
                )
               .AddScoped(sp => sp.GetRequiredService<NpgsqlDataSource>().CreateConnection());
    }

    public static async Task SetupDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = typeof(DatabaseAccessModule).GetEmbeddedResource("DatabaseSetup.sql");
        await command.ExecuteNonQueryAsync();
    }
}