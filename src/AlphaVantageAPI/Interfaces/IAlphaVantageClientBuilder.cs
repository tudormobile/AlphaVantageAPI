using Microsoft.Extensions.Logging;

namespace Tudormobile.AlphaVantage;

/// <summary>
/// Defines a builder for configuring and creating instances of <see cref="IAlphaVantageClient"/>.
/// </summary>
/// <remarks>Use this interface to fluently configure API credentials and other options before constructing an
/// <see cref="IAlphaVantageClient"/>. The builder pattern enables step-by-step configuration and validation prior to
/// client creation.</remarks>
public interface IAlphaVantageClientBuilder : IBuilder<IAlphaVantageClient>
{
    /// <summary>
    /// Sets the API key to be used for authenticating requests to the Alpha Vantage service.
    /// </summary>
    /// <param name="apiKey">The API key provided by Alpha Vantage. Cannot be null or empty.</param>
    /// <returns>The current builder instance with the specified API key configured.</returns>
    IAlphaVantageClientBuilder WithApiKey(string apiKey);

    /// <summary>
    /// Configures the builder to use the specified HTTP client for sending requests to the Alpha Vantage API.
    /// </summary>
    /// <remarks>Use this method to provide a custom-configured HttpClient, for example to set custom headers,
    /// proxies, or timeouts. If not set, a default HttpClient may be used by the implementation.</remarks>
    /// <param name="httpClient">The HTTP client instance to use for all outgoing API requests. Cannot be null. The caller is responsible for
    /// managing the lifetime of the provided client.</param>
    /// <returns>The current builder instance configured to use the specified HTTP client.</returns>
    IAlphaVantageClientBuilder WithHttpClient(HttpClient httpClient);

    /// <summary>
    /// Adds the specified logger to the client builder for capturing diagnostic and operational messages.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging client activity. Cannot be null.</param>
    /// <returns>The current instance of the client builder for method chaining.</returns>
    IAlphaVantageClientBuilder AddLogging(ILogger logger);
}
