using Microsoft.AspNetCore.Builder;
using Serilog;

namespace WebApp.CompositionRoot;

public static class Middleware
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.MapHealthChecks("/");
        return app;
    }
}