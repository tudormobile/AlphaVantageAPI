# Introduction

```
using Tudormobile.AlphaVantage;
```

## Tudormobile.AlphaVantage
This namespace contains the basic building blocks for accessing the Alpha Vantage API.

The basic model consists of a lightweight wrapper around the Alpha Vantage web services. Use the *AlphaVantageClient* to retrieve either *JsonDocument* or *JsonString* from the service. You must provide your *apikey* to the constructor. All methods are asynchronous.
```cs
var apiKey = "demo";                // demo API key for testing
var httpClient = new HttpClient();  // Http client to use
var client = new AlphaVantageClient(apiKey, httpClient);
var function = AlphaVantageFunction.GLOBAL_QUOTE;
var symbol = "IBM";

var jsonString = await client.GetJsonStringAsync(function, symbol);
// or..
var jsonDocument = await client.GetJsonDocumentAsync(function, symbol);
```
> [!NOTE]
> Only the free tier api functions are currently implemented.

The basic model makes no attempt to modify or extend the AlphaVantage api in any way and serves only as a lightweight wrapper to make it slightly easier to consume Json data from the service in the c# language. All responses, including error responses, are passed through unchanged. See the Alpha Vantage API documentation for a complete description.

HttpClient management is the responsibility of the host application.

### Tudormobile.AlphaVantage.Extensions
The extensibility model provides the building blocks for an extensible implementation, including interfaces for the *IAlphaVantageClient*, a builder pattern, and additional methods that match the function being called (e.g., *GlobalQuote*) rather than providing a function argument to the low-level calls. 
```cs
var client = AlphaVantageClient.GetBuilder()
            .WithApiKey("demo")
            .WithHttpClient(new HttpClient())
            .Build();
var jsonDocument = await client.GlobalQuoteJsonAsync("IBM");
```
The extensibility model also goes further to provide an object model for Alpha Vantage responses consisting of a uniform set of entities and data structures to represent the data.
```cs
var client = AlphaVantageClient.GetBuilder()
            .WithApiKey("demo")
            .WithHttpClient(new HttpClient())
            .Build();

AlphaVantageResponse<GlobalQuote> response = await client.GetGlobalQuoteAsync("IBM");
if (response.IsSuccess)
{
    GlobalQuote quote = response.Result!;
    Console.WriteLine($"The price of {quote.Symbol} is {quote.Price:C}");
}
else 
{
    Console.WriteLine($"Error: {response.ErrorMessage}");
}
```
Some calls can be aggregated to provide additional functionality from the api as well.
```cs
var client = AlphaVantageClient.GetBuilder()
    .WithApiKey("demo")
    .WithHttpClient(new HttpClient())
    .Build();
var data = await client.GetGlobalQuotesAsync(["IBM", "APPL", "MSFT"]);

// data is an IDictionary<String, AlphaVantageResponse<GlobalQuote>>
```

### Dependency Injects
The AlphaVanatageAPI library takes advanatage of the dotnet dependency inject model, extending the IServiceCollection to provide an implementation of IAlphaVantageClient that can be added to the collection using *AddAlphaVantageClient()* extension method.
```cs
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();           // required for AlphaVantageClient to use
builder.Services.AddAlphaVantageClient();
```

> [!NOTE]
> Only the free tier api calls are currently implemented.
