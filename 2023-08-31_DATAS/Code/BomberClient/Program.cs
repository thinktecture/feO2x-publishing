using System;
using System.Net.Http;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace BomberClient;

public static class Program
{
    public static void Main()
    {
        const int numberOfCallsPerInterval = 300;
        var interval = TimeSpan.FromSeconds(1);

        using var httpClient = new HttpClient();
        var scenario =
            Scenario
               .Create(
                    "bomb_web_app",
                    async _ =>
                    {
                        var request = Http.CreateRequest("GET", "http://localhost:5000/api/call");

                        // ReSharper disable once AccessToDisposedClosure
                        // HttpClient will not be disposed when this lambda is called
                        return await Http.Send(httpClient, request);
                    })
               .WithoutWarmUp()
               .WithLoadSimulations(
                    Simulation.RampingInject(numberOfCallsPerInterval, interval, TimeSpan.FromSeconds(20)),
                    Simulation.Inject(numberOfCallsPerInterval, interval, TimeSpan.FromMinutes(7)),
                    Simulation.RampingInject(0, interval, TimeSpan.FromSeconds(10))
                );

        NBomberRunner.RegisterScenarios(scenario).Run();
    }
}