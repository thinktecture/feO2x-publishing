using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApp;

public static class Endpoint
{
    private static ulong _numberOfCalls;
    private static int[]? _currentArray;

    public static void MapEndpoint(this WebApplication app)
    {
        app.MapGet(
            "/api/call",
            () =>
            {
                var numberOfCalls = Interlocked.Increment(ref _numberOfCalls);
                if (numberOfCalls != 0 && numberOfCalls % 1000 == 0)
                {
                    var largeArray = new int[30_000];
                    Interlocked.Exchange(ref _currentArray, largeArray);
                }

                return Results.Ok(new NumberOfCallsDto(numberOfCalls));
            }
        );
    }
}

public sealed record NumberOfCallsDto(ulong NumberOfCalls);