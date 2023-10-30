using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;

long initialTimestamp = Stopwatch.GetTimestamp();
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
await app.StartAsync();

TimeSpan elapsedStartupTime = Stopwatch.GetElapsedTime(initialTimestamp);
Console.WriteLine($"Startup took {elapsedStartupTime.TotalMilliseconds:N3}ms");

double workingSet = Process.GetCurrentProcess().WorkingSet64;
Console.WriteLine($"Working Set: {workingSet / (1024 * 1024):N2}MB");

await app.StopAsync();