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
        _httpClient!.Dispose();
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
    public void Constructor_WithNullHttpClient_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => new AlphaVantageClient("key", null!));
        Assert.AreEqual("httpClient", ex.ParamName);
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
    public async Task GetJsonStringAsync_WithNullSymbol_ThrowsArgumentException()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        string? symbol = null;
        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => client.GetJsonStringAsync(function, symbol!, TestContext.CancellationToken));
        Assert.AreEqual("symbol", ex.ParamName);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithNullSymbol_ThrowsArgumentException()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey, _httpClient!);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        string? symbol = null;
        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => client.GetJsonDocumentAsync(function, symbol!, TestContext.CancellationToken));
        Assert.AreEqual("symbol", ex.ParamName);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_HttpClientThrowsException()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysThrows = new Exception("ExceptionMessage") };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken));
        Assert.AreEqual("ExceptionMessage", ex.Message);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_HttpClientThrowsException()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysThrows = new Exception("ExceptionMessage") };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken));
        Assert.AreEqual("ExceptionMessage", ex.Message);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_HttpClientTooManyRequests()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysResponds = new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests) };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsExactlyAsync<AlphaVantageException>(() => client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken));
        Assert.AreEqual("Rate limit exceeded", ex.Message);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_HttpClientTooManyRequests()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysResponds = new HttpResponseMessage(System.Net.HttpStatusCode.TooManyRequests) };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.SYMBOL_SEARCH;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsExactlyAsync<AlphaVantageException>(() => client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken));
        Assert.AreEqual("Rate limit exceeded", ex.Message);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_HttpClientUnauthorized()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysResponds = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsExactlyAsync<AlphaVantageException>(() => client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken));
        Assert.AreEqual("Invalid API key", ex.Message);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_InternalServerError()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysResponds = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError) };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsExactlyAsync<AlphaVantageException>(() => client.GetJsonStringAsync(function, symbol, TestContext.CancellationToken));
        Assert.StartsWith("Failed to fetch function", ex.Message);
        Assert.Contains("symbol", ex.Message);
        Assert.IsInstanceOfType(ex.InnerException, typeof(HttpRequestException));
        Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, ((HttpRequestException)ex.InnerException).StatusCode);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_InternalServerError()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var mockHandler = new MockHttpMessageHandler() { AlwaysResponds = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError) };
        using var httpClient = new HttpClient(mockHandler);
        var client = new AlphaVantageClient(apiKey, httpClient);
        var function = AlphaVantageFunction.SYMBOL_SEARCH;
        var symbol = "IBM";
        // Act & Assert
        var ex = await Assert.ThrowsExactlyAsync<AlphaVantageException>(() => client.GetJsonDocumentAsync(function, symbol, TestContext.CancellationToken));
        Assert.StartsWith("Failed to fetch function", ex.Message);
        Assert.Contains("keywords", ex.Message);
        Assert.IsInstanceOfType(ex.InnerException, typeof(HttpRequestException));
        Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, ((HttpRequestException)ex.InnerException).StatusCode);
    }

    public TestContext TestContext { get; set; }
}
