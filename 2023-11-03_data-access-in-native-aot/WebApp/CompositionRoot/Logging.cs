using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace WebApp.CompositionRoot;

public static class Logging
{
    public static ILogger CreateLogger() =>
        new LoggerConfiguration().MinimumLevel.Information()
                                 .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                                 .WriteTo.Console().CreateLogger();
    
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.Host.UseSerilog(logger);
        return builder;
    }
}