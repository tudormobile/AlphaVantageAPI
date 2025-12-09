# Introduction

```
using Tudormobile.AlphaVantage;
```

## Tudormobile.AlphaVantage
This namespace contains the basic building blocks for accessing the Alpha Vanatage API.

The basic model consists of a lightweight wrapper around the Alpha Vantage web services. Use the *AlphaVantageClient* to retrieve either *JsonDocument* or *JasonString* from the service. You must povide your *apikey* to the constructor. All methods are asynchronous.
```cs
var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
var client = new AlphaVantageClient(apiKey);
var function = AlphaVantageFunction.GLOBAL_QUOTE;
var symbol = "IBM";

var jsonString = await client.GetJsonStringAsync(function, symbol);
// or..
var jsonDocument = await client.GetJsonDocumentAsync(function, symbol);
```
> [!NOTE]
> Only the free tier api functions are currently implemented.

The basic model makes no attempt to modify or extend the AlphaVantage api in any way and serves only as a lightweight wrapper to make it slighlty easier to consume Json data from the service in the c# language. All responses, including error responses, are passed through unchanged. See the Alpha Vanatage API documentation for a complete description.

### Tudormobile.AlphaVanatage.Extensions
The extensibility model provides the building blocks for an extensible implementation, including interfaces for the *IAlphaVantageClient*, a builder pattern, and additional methods that match the function being called (e.g., *GlobalQuote*) rather than providing a function argument to the low-level calls. 
```cs
var client = AlphaVantageClient.GetBuilder()
            .WithApiKey("demo")
            .Build();
var jsonDocument = await client.GlobalQuoteAsync("IBM");
```
The extensibility model also goes further to provide an object model for Alpha Vanatage responses consisting of a uniform set of entities and data structures to represent the data.
```cs
var client = AlphaVantageClient.GetBuilder()
            .WithApiKey("demo")
            .Build();

AlphaVantageResponse<GlobalQuote> response = client.GlobalQuote("IBM");
if (response.IsSuccess)
{
    GlobalQuote quote = response.Result;
    Console.WriteLine($"The price of {quote.Symbol} is {quote.Price:C}");
}
else 
{
    Console.WriteLine("$Error: {response.ErrorMessage}");
}
```
Some calls can be aggregated to provide additional functionality from the api as well.
```cs
var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").Build();
var data = await client.GlobalQuotes(["IBM", "APPL", "MSFT"]);

// data is an IDictionary<String, AlphaVantageResponse<GlobalQuote>>
```

> [!NOTE]
> Only the free tier api calls are currently implemented.
