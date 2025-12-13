using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Tudormobile.AlphaVantage;

/// <summary>
/// Provides methods for accessing financial data from the Alpha Vantage API.
/// </summary>
/// <remarks>
/// Use this client to retrieve stock quotes, time series data, and other financial information from
/// Alpha Vantage. An API key from Alpha Vantage is required to use most features.
/// </remarks>
public class AlphaVantageClient : IAlphaVantageClient
{
    private readonly string _apiKey;
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AlphaVantageClient class using the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key used to authenticate requests to the Alpha Vantage service. Cannot be null or empty.</param>
    /// <param name="httpClient">The HttpClient instance used to make HTTP requests to the Alpha Vantage API.</param>
    /// <param name="logger">Optional logger instance for logging diagnostic information. If null, a NullLogger will be used.</param>
    public AlphaVantageClient(string apiKey, HttpClient httpClient, ILogger? logger = null)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
        }
        _apiKey = apiKey;
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger.LogDebug("AlphaVantageClient initialized.");
    }

    /// <summary>
    /// Initializes a new instance of the AlphaVantageClient class using the specified HTTP client factory, API key, and
    /// logger.
    /// </summary>
    /// <param name="httpClientFactory">The factory used to create HTTP client instances for sending requests to the Alpha Vantage API. Cannot be null.</param>
    /// <param name="apiKey">The API key used to authenticate requests to the Alpha Vantage service. Cannot be null or empty.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for the AlphaVantageClient. Cannot be null.</param>
    public AlphaVantageClient(IHttpClientFactory httpClientFactory, string apiKey, ILogger<AlphaVantageClient> logger)
        : this(apiKey, httpClientFactory.CreateClient(nameof(AlphaVantageClient)), logger) { }

    /// <summary>
    /// Asynchronously retrieves the raw JSON response from the Alpha Vantage API for the specified function and symbol.
    /// </summary>
    /// <param name="function">The Alpha Vantage function to query. Determines the type of data returned by the API.</param>
    /// <param name="symbol">The symbol representing the financial instrument to query. Cannot be null or empty.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the JSON response as a string.</returns>
    public async Task<string> GetJsonStringAsync(
        AlphaVantageFunction function,
        string symbol,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol cannot be null or empty.", nameof(symbol));
        }

        _logger.LogDebug("Requesting json data {Function} for {Symbol}", function, symbol);
        try
        {
            var stopwatch = Stopwatch.StartNew();
            using var reader = new StreamReader(await GetStreamAsync(function, symbol, cancellationToken).ConfigureAwait(false));
            var result = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully fetched {Function} json data for {Symbol} in {ElapsedMs}ms", function, symbol, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching {Function} for {Symbol}", function, symbol);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously retrieves and parses a JSON document for the specified Alpha Vantage function and symbol.
    /// </summary>
    /// <param name="function">The Alpha Vantage function to query. Determines the type of data to retrieve.</param>
    /// <param name="symbol">The symbol representing the financial instrument to query. Cannot be null or empty.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a JsonDocument representing the
    /// parsed JSON response.</returns>
    public async Task<JsonDocument> GetJsonDocumentAsync(
        AlphaVantageFunction function,
        string symbol,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol cannot be null or empty.", nameof(symbol));
        }

        _logger.LogDebug("Requesting JsonDocument {Function} for {Symbol}", function, symbol);
        try
        {
            var stopwatch = Stopwatch.StartNew();
            using var stream = await GetStreamAsync(function, symbol, cancellationToken).ConfigureAwait(false);
            var result = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully fetched {Function} data for {Symbol} in {ElapsedMs}ms", function, symbol, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching {Function} for {Symbol}", function, symbol);
            throw;
        }
    }

    private async Task<Stream> GetStreamAsync(
        AlphaVantageFunction function,
        string symbolOrKeywords,
        CancellationToken cancellationToken = default)
    {
        var symOrKey = function == AlphaVantageFunction.SYMBOL_SEARCH
            ? $"&keywords={Uri.EscapeDataString(symbolOrKeywords)}"
            : $"&symbol={Uri.EscapeDataString(symbolOrKeywords)}";
        var url = $"https://www.alphavantage.co/query?function={function}{symOrKey}&apikey={Uri.EscapeDataString(_apiKey)}";
        var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        try
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            response.Dispose();
            var label = function == AlphaVantageFunction.SYMBOL_SEARCH ? "keywords" : "symbol";
            _logger.LogWarning("Rate limit hit for {Function} - {Label}: {Value}", function, label, symbolOrKeywords);
            throw new AlphaVantageException("Rate limit exceeded", ex);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            response.Dispose();
            throw new AlphaVantageException("Invalid API key", ex);
        }
        catch (HttpRequestException ex)
        {
            response.Dispose();
            var label = function == AlphaVantageFunction.SYMBOL_SEARCH ? "keywords" : "symbol";
            throw new AlphaVantageException($"Failed to fetch function '{function}' for {label} '{symbolOrKeywords}': {ex.Message}", ex);
        }
    }
}

