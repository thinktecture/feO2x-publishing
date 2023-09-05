using System;
using System.Runtime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace WebApp;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                                              .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                              .WriteTo.Console()
                                              .CreateLogger();
        
        if (GCSettings.IsServerGC)
            Log.Information("Running in Server GC mode");
        else
            Log.Information("Running in Workstation GC mode");
        
        Log.Information("Process ID {ProcessId}", Environment.ProcessId);
        
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog(Log.Logger);
            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.MapHealthChecks("/");
            app.MapEndpoint();

            await app.RunAsync();
            return 0;
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Could not run web app");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}


