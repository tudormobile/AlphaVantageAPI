using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tudormobile.AlphaVantage;
using Tudormobile.AlphaVantage.Extensions;

namespace ExtendedConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // Setup Host with AlphaVantage Client
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddAlphaVantageClient(builder => builder.ApiKey = "demo")
            .AddLogging(builder => builder.AddConsole());

        using IHost host = builder.Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        // Grab a client and get a quote
        var symbol = "IBM";
        var client = host.Services.GetRequiredService<IAlphaVantageClient>();
        var quoteTask = client.GetGlobalQuoteAsync(symbol);

        quoteTask.Wait();

        logger.LogInformation("GetGlobalTask result: {result}", quoteTask.Result);
    }
}
