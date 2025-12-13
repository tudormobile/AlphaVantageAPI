using System.Diagnostics.CodeAnalysis;
using Tudormobile.AlphaVantage;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageClientBuilderTests
{
    [TestMethod]
    public void WithApiKey_SetsApiKey()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var apiKey = "test-api-key-123";

        // Act
        var result = builder.WithApiKey(apiKey);

        // Assert
        Assert.AreEqual(apiKey, builder.ApiKey);
    }

    [TestMethod]
    public void WithLogger_SetsLogger()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var logger = new MockLogger();

        // Act
        var result = builder.AddLogging(logger);

        // Assert
        Assert.AreEqual(logger, builder.Logger);
    }

    [TestMethod]
    public void AddLogger_ReturnsBuilderInstance_ForFluentChaining()
    {
        var builder = new AlphaVantageClientBuilder();

        // Act
        var result = builder.AddLogging(new MockLogger());

        // Assert
        Assert.AreSame(builder, result);

    }

    [TestMethod]
    public void WithApiKey_ReturnsBuilderInstance_ForFluentChaining()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();

        // Act
        var result = builder.WithApiKey("test-key");

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void WithHttpClient_ReturnsBuilderInstance_ForFluentChaining()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();

        // Act
        var result = builder.WithHttpClient(httpClient);

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void ApiKey_Property_CanBeSetDirectly()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var apiKey = "direct-set-key";

        // Act
        builder.ApiKey = apiKey;

        // Assert
        Assert.AreEqual(apiKey, builder.ApiKey);
    }

    [TestMethod]
    public void ApiKey_Property_DefaultsToEmptyString()
    {
        // Arrange & Act
        var builder = new AlphaVantageClientBuilder();

        // Assert
        Assert.AreEqual(string.Empty, builder.ApiKey);
    }

    [TestMethod]
    public void Build_WithValidConfiguration_ReturnsAlphaVantageClient()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();
        builder.WithApiKey("valid-api-key")
               .WithHttpClient(httpClient);

        // Act
        var client = builder.Build();

        // Assert
        Assert.IsNotNull(client);
        Assert.IsInstanceOfType<AlphaVantageClient>(client);
    }

    [TestMethod]
    public void Build_WithoutApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();
        builder.WithHttpClient(httpClient);

        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            builder.Build();
        });

        Assert.Contains("API key must be set", exception.Message);
        Assert.Contains("WithApiKey()", exception.Message);
    }

    [TestMethod]
    public void Build_WithEmptyApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();
        builder.WithApiKey(string.Empty)
               .WithHttpClient(httpClient);

        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            builder.Build();
        });

        Assert.Contains("API key must be set", exception.Message);
    }

    [TestMethod]
    public void Build_WithWhitespaceApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();
        builder.WithApiKey("   ")
               .WithHttpClient(httpClient);

        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            builder.Build();
        });

        Assert.Contains("API key must be set", exception.Message);
    }

    [TestMethod]
    public void Build_WithoutHttpClient_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        builder.WithApiKey("valid-api-key");

        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            builder.Build();
        });

        Assert.Contains("HttpClient instance must be provided", exception.Message);
        Assert.Contains("WithHttpClient()", exception.Message);
    }

    [TestMethod]
    public void WithApiKey_CanBeCalledMultipleTimes_LastValueWins()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        builder.WithApiKey("first-key");
        builder.WithApiKey("second-key");

        // Act
        var finalKey = builder.ApiKey;

        // Assert
        Assert.AreEqual("second-key", finalKey);
    }

    [TestMethod]
    public void WithHttpClient_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient1 = new HttpClient();
        var httpClient2 = new HttpClient();

        // Act
        builder.WithHttpClient(httpClient1);
        builder.WithHttpClient(httpClient2);
        var client = builder.WithApiKey("test-key").Build();

        // Assert - Should not throw, last HttpClient wins
        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void Build_WithMinimalConfiguration_WorksWithoutLogger()
    {
        // Arrange
        var builder = new AlphaVantageClientBuilder();
        var httpClient = new HttpClient();

        // Act
        var client = builder
            .WithApiKey("minimal-key")
            .WithHttpClient(httpClient)
            .Build();

        // Assert
        Assert.IsNotNull(client);
    }

    [ExcludeFromCodeCoverage]
    class MockLogger : Microsoft.Extensions.Logging.ILogger
    {
#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public IDisposable BeginScope<TState>(TState state) => null!;
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // No-op
        }
    }
}
