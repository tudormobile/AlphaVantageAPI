using System.Reflection;
using Tudormobile.AlphaVantage;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageClientTests
{
    private static HttpClient? _httpClient;
    private static HttpMessageHandler? _mockHttpMessageHandler;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _mockHttpMessageHandler = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttpMessageHandler) { Timeout = TimeSpan.FromSeconds(30) };
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

    public TestContext TestContext { get; set; }
}
