# AlphaVantageAPI
Alpha Vantage API library

[![Build and Deploy](https://github.com/tudormobile/AlphaVantageAPI/actions/workflows/dotnet.yml/badge.svg)](https://github.com/tudormobile/AlphaVantageAPI/actions/workflows/dotnet.yml)  [![Publish Docs](https://github.com/tudormobile/AlphaVantageAPI/actions/workflows/docs.yml/badge.svg)](https://github.com/tudormobile/AlphaVantageAPI/actions/workflows/docs.yml)

Copyright (C) 2025 Bill Tudor  

A lightweight library to access the Apha Vantage API.

> [!NOTE]  
> The current state is pre-production (v0.x).

### Quick Start

#### Low-level (Basic) api:
```cs
using Tudormobile.AlphaVantage;

var apiKey = "demo";    // your api key
var httpClient = new HttpClient();
var client = new AlphaVantageClient(apiKey, httpClient);
var function = AlphaVantageFunction.GLOBAL_QUOTE;
var symbol = "IBM";
// utf8json string...
var json = await client.GetJsonStringAsync(function, symbol);
// Or document...
var doc = await client.GetJsonDocumentAsync(function, symbol);
```
[!TIP]: See the sample '*SimpleConsoleApp*'.

#### Using the extensions:
```cs
using Tudormobile.AlphaVantage;
using Tudormobile.AlphaVantage.Extensions;

var apiKey = "demo";    // your api key
var httpClient = new HttpClient();
var client = AlphaVantageClient.GetBuilder()
             .WithApiKey(apiKey)
             .WithHttpClient(httpClient)
             .Build();
var symbol = "IBM";
var json = client.GlobalQuote(symbol);
```
#### Using dependency injection:
```cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tudormobile.AlphaVantage;
using Tudormobile.AlphaVantage.Extensions;

var apiKey = "demo";    // your api key
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
using IHost host = builder.Services
    .AddAlphaVantageClient(options => options.ApiKey = "demo")
    .Build();

// Grab a client and get a quote
var symbol = "IBM";
var client = host.Services.GetRequiredService<IAlphaVantageClient>();
var quoteTask = client.GetGlobalQuoteAsync(symbol);

quoteTask.Wait();

var quote = quoteTask.Result;   

// Quote is an entity from the object model
decimal price = quote.Price;
/// ...
```
[!TIP]: See the sample '*ExtendedConsoleApp*'.

> [!NOTE]
> Only the free tier Core Stock API is currently implemented.

[NuGET Package README](docs/README.md) | [Source Code README](src/README.md) | [API Documentation](https://tudormobile.github.io/AlphaVantageAPI/)
