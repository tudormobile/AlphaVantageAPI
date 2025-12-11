using System.Reflection;
using Tudormobile.AlphaVantage;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageClientTests
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
    public void Constructor_WithValidApiKey_SetsApiKeyField()
    {
        // Arrange
        var expected = "test-api-key-123";

        // Act
        var client = new AlphaVantageClient(expected, _httpClient!);

        // Assert
        var field = typeof(AlphaVantageClient).GetField("_apiKey", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field);
        var actual = field.GetValue(client) as string;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Constructor_WithNullApiKey_ThrowsArgumentException()
    {
        // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient(null, _httpClient!));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod]
    public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient(string.Empty, _httpClient!));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod]
    public void Constructor_WithWhitespaceApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient("   ", _httpClient!));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod]
    public void Constructor_WithWhitespaceTabsApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient("\t\t", _httpClient!));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithValidParameters()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Global Quote", out _));
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithInvalidParameters()
    {
        // Arrange
        var apiKey = "invalid-key";
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "invalid-symbol";    // Use something other than IBM to avoid demo success
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Global Quote", out var quote));
        Assert.IsTrue(quote.GetRawText().Trim().Equals("{}"));
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithInvalidDemoKey()
    {
        // Arrange
        var apiKey = "demo";
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "invalid-symbol";    // Use something other than IBM to avoid demo success
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsFalse(jsonDocument.RootElement.TryGetProperty("Global Quote", out _));
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Information", out _));
    }

    [TestMethod]
    public async Task GetJsonStringAsync_WithValidParameters()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act
        var jsonString = await client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken);
        // Assert
        Assert.IsNotNull(jsonString);
        Assert.Contains("\"Global Quote\"", jsonString);
        Assert.Contains("\"IBM\"", jsonString);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_SymbolSearch_ReturnsValidJson()
    {
        // Arrange
        var apiKey = "test-key"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.SYMBOL_SEARCH;
        var symbol = "Microsoft Corp";
        // Act
        var jsonString = await client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken);
        // Assert
        Assert.IsNotNull(jsonString);
        Assert.Contains("\"bestMatches\"", jsonString);
        Assert.Contains("\"MSFT\"", jsonString);
    }

    public TestContext TestContext { get; set; }
}

