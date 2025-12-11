using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
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

        var quote = quoteTask.Result;

        logger.LogInformation("GetGlobalTask IsSuccess: '{success}'", quote.IsSuccess);
        logger.LogInformation("GetGlobalTask ErrorMessaage: '{message}'", quote.ErrorMessage);
        logger.LogInformation("GetGlobalTask Entity: '{type}'", quote.Result?.GetType().Name ?? "(null)");

        // print some result details
        if (quote.Result is not null)
        {
            logger.LogInformation("GetGlobalTask LatestTradingDay: '{day}'", quote.Result.LatestTradingDay);
            logger.LogInformation("GetGlobalTask Symbol: '{symbol}'", quote.Result.Symbol);
            logger.LogInformation("GetGlobalTask Price: '{price}'", quote.Result.Price);
            logger.LogInformation("GetGlobalTask Volume: '{volume}'", quote.Result.Volume);
            logger.LogInformation("GetGlobalTask Open: '{open}'", quote.Result.Open);
            logger.LogInformation("GetGlobalTask PreviousClose: '{close}'", quote.Result.PreviousClose);
            logger.LogInformation("GetGlobalTask High: '{high}'", quote.Result.High);
            logger.LogInformation("GetGlobalTask Low: '{low}'", quote.Result.Low);
            logger.LogInformation("GetGlobalTask Change: '{change}'", quote.Result.Change);
            logger.LogInformation("GetGlobalTask ChangePercent: '{percent}'", quote.Result.ChangePercent);

            Console.WriteLine("Full Quote Result:");
            Console.WriteLine(JsonSerializer.Serialize(quote.Result, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
