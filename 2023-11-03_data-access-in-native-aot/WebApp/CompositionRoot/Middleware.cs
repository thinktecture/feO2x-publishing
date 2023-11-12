using Microsoft.AspNetCore.Builder;
using Serilog;
using WebApp.Contacts;

namespace WebApp.CompositionRoot;

public static class Middleware
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseRouting();
        app.MapContactEndpoints();
        app.MapHealthChecks("/");
        return app;
    }
}