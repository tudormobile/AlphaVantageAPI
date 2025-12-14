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
    /// Asynchronously retrieves the raw JSON response from the Alpha Vantage API for the specified function and query parameters.
    /// </summary>
    /// <param name="function">The Alpha Vantage function to query. Determines the type of data returned by the API.</param>
    /// <param name="queryParameters">A dictionary of query parameters to include in the API request. Cannot be null or empty.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the JSON response as a string.</returns>
    public async Task<string> GetJsonStringAsync(
        AlphaVantageFunction function,
        IDictionary<string, string> queryParameters,
        CancellationToken cancellationToken = default)
    {
        if (queryParameters == null || queryParameters.Count == 0)
        {
            throw new ArgumentException("Query parameters cannot be null or empty.", nameof(queryParameters));
        }

        _logger.LogDebug("Requesting json data {Function} for {Query}", function, AsString(queryParameters));
        try
        {
            var stopwatch = Stopwatch.StartNew();
            using var reader = new StreamReader(await GetStreamAsync(function, queryParameters, cancellationToken).ConfigureAwait(false));
            var result = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully fetched {Function} json data for {Symbol} in {ElapsedMs}ms", function, AsString(queryParameters), stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching {Function} for {Query}", function, AsString(queryParameters));
            throw;
        }
    }

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
    /// Asynchronously retrieves a JSON document from the Alpha Vantage API using the specified function and query
    /// parameters.
    /// </summary>
    /// <remarks>The caller is responsible for disposing the returned JsonDocument. The method throws if the
    /// HTTP request fails or if the response cannot be parsed as JSON.</remarks>
    /// <param name="function">The Alpha Vantage function to invoke when constructing the API request.</param>
    /// <param name="queryParameters">A dictionary containing the query parameters to include in the API request. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a JsonDocument with the data
    /// returned by the Alpha Vantage API.</returns>
    /// <exception cref="ArgumentException">Thrown if queryParameters is null or contains no entries.</exception>
    public async Task<JsonDocument> GetJsonDocumentAsync(
        AlphaVantageFunction function,
        IDictionary<string, string> queryParameters,
        CancellationToken cancellationToken = default)
    {
        if (queryParameters == null || queryParameters.Count == 0)
        {
            throw new ArgumentException("Query parameters cannot be null or empty.", nameof(queryParameters));
        }
        _logger.LogDebug("Requesting JsonDocument {Function} with parameters {Parameters}", function, AsString(queryParameters));
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var symOrKey = string.Join("&", queryParameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            var url = $"https://www.alphavantage.co/query?function={function}&{symOrKey}&apikey={Uri.EscapeDataString(_apiKey)}";
            var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            var result = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Successfully fetched {Function} data with parameters in {ElapsedMs}ms", function, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching {Function} with parameters", function);
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
        IEnumerable<KeyValuePair<string, string>> queryParameters,
        CancellationToken cancellationToken = default)
    {
        var uri = string.Concat(
            $"https://www.alphavantage.co/query?function={function}&apikey={Uri.EscapeDataString(_apiKey)}&",
            string.Join("&", queryParameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"))
           );
        var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        try
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            _logger.LogWarning("Rate limit hit for {Function} - Query Parameters: {Value}", function, AsString(queryParameters));
            response.Dispose();
            throw new AlphaVantageException("Rate limit exceeded", ex);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            response.Dispose();
            throw new AlphaVantageException("Invalid API key", ex);
        }
        catch (HttpRequestException ex)
        {
            var message = $"Failed to fetch function '{function}' for query '{AsString(queryParameters)}': {ex.Message}";
            response.Dispose();
            throw new AlphaVantageException(message, ex);
        }
    }

    private static string AsString(IEnumerable<KeyValuePair<string, string>> queryParameters)
    {
        if (queryParameters == null) return "(none)";
        return string.Join(',', queryParameters.Select(kv => $"{kv.Key}={kv.Value}"));
    }

    private async Task<Stream> GetStreamAsync(
        AlphaVantageFunction function,
        string symbolOrKeywords,
        CancellationToken cancellationToken = default)
    {
        KeyValuePair<string, string> symOrKey = function == AlphaVantageFunction.SYMBOL_SEARCH
            ? new("keywords", Uri.EscapeDataString(symbolOrKeywords))
            : new("symbol", Uri.EscapeDataString(symbolOrKeywords));
        return await GetStreamAsync(function, [symOrKey], cancellationToken);
    }

}

