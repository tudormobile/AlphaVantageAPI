using System.Reflection;
using System.Text.Json;
using Tudormobile.AlphaVantage;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageClientExtensionsTests
{
    private static HttpClient? _httpClient;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _httpClient?.Dispose();
    }

    [TestMethod]
    public void GetBuilderTest()
    {
        var builder = AlphaVantageClient.GetBuilder();
        Assert.IsInstanceOfType<IBuilder<IAlphaVantageClient>>(builder);
    }

    [TestMethod]
    public void BuildTest()
    {
        var expected = "test-api-key-123";
        var target = AlphaVantageClient.GetBuilder()
            .WithApiKey(expected)
            .WithHttpClient(_httpClient!)
            .Build();
        // Assert
        var field = typeof(AlphaVantageClient).GetField("_apiKey", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field);
        var actual = field.GetValue(target) as string;
        Assert.AreEqual(expected, actual);

    }

    [TestMethod]
    public async Task GlobalQuoteTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.GlobalQuoteJsonAsync("IBM", TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task TimeSeriesDailyTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.TimeSeriesDailyJsonAsync("IBM", TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task TimeSeriesMonthlyTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.TimeSeriesMonthlyJsonAsync("IBM", cancellationToken: TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task TimeSeriesMonthlyAdjustedTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.TimeSeriesMonthlyJsonAsync("IBM", adjusted: true, TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task TimeSeriesWeeklyTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.TimeSeriesWeeklyJsonAsync("IBM", cancellationToken: TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task TimeSeriesWeeklyAdjustedTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.TimeSeriesWeeklyJsonAsync("IBM", adjusted: true, TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task SymbolSearchTest()
    {
        var expected = JsonValueKind.Object;
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var doc = await client.SymbolSearchJsonAsync("IBM", TestContext.CancellationToken);
        var actual = doc.RootElement;
        Assert.AreEqual(expected, actual.ValueKind);
    }

    [TestMethod]
    public async Task GlobalQuoteEntityTest()
    {
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var actual = await client.GetGlobalQuoteAsync("IBM", TestContext.CancellationToken);
        Assert.IsTrue(actual.IsSuccess);
        Assert.AreEqual("IBM", actual.Result!.Symbol);
    }

    [TestMethod]
    public async Task GlobalQuoteEntityTestWithError()
    {
        // note: the demo API key only works with IBM symbol
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var actual = await client.GetGlobalQuoteAsync("ABCDEFG", TestContext.CancellationToken);
        Assert.IsFalse(actual.IsSuccess);
        Assert.IsNotNull(actual.ErrorMessage);
        Assert.IsNull(actual.Result);
    }

    [TestMethod]
    public async Task GlobalQuoteEntityTestWithMultipleSymbols()
    {
        var client = AlphaVantageClient.GetBuilder().WithApiKey("demo").WithHttpClient(_httpClient!).Build();
        var actual = await client.GetGlobalQuotesAsync(["IBM", "APPL", "MSFT"], TestContext.CancellationToken);
        Assert.HasCount(3, actual);
    }

    public TestContext TestContext { get; set; }
}