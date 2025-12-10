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
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(30) };

    /// <summary>
    /// Initializes a new instance of the AlphaVantageClient class using the specified API key.
    /// </summary>
    /// <param name="apiKey">The API key used to authenticate requests to the Alpha Vantage service. Cannot be null or empty.</param>
    public AlphaVantageClient(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
        }
        _apiKey = apiKey;
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
        using var reader = new StreamReader(await GetStreamAsync(function, symbol, cancellationToken).ConfigureAwait(false));
        return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
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
        using var stream = await GetStreamAsync(function, symbol, cancellationToken).ConfigureAwait(false);
        return await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
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
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
    }
}

