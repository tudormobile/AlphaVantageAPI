using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

/// <summary>
/// Provides extension methods for working with instances of <see cref="IAlphaVantageClient"/>.
/// </summary>
/// <remarks>This class contains static methods that extend the functionality of <see cref="IAlphaVantageClient"/>
/// implementations, enabling fluent configuration and builder patterns. These extensions are intended to simplify
/// client setup and integration scenarios.</remarks>
public static class AlphaVantageClientExtensions
{
    extension(IAlphaVantageClient)
    {
        /// <summary>
        /// Creates a new builder for configuring and constructing an instance of an Alpha Vantage client.
        /// </summary>
        /// <returns>An <see cref="IBuilder{IAlphaVantageClient}"/> that can be used to configure and build an <see
        /// cref="IAlphaVantageClient"/> instance.</returns>
        public static IAlphaVantageClientBuilder GetBuilder()
        {
            return new AlphaVantageClientBuilder();
        }
    }

    /// <summary>
    /// Retrieves daily time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve daily time series data for.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the daily time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesDailyAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.TIME_SERIES_DAILY, symbol);

    /// <summary>
    /// Retrieves monthly time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve monthly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted monthly data; otherwise, returns unadjusted monthly data. Default is false.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the monthly time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesMonthlyAsync(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_MONTHLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_MONTHLY, symbol);

    /// <summary>
    /// Retrieves weekly time series data for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve weekly time series data for.</param>
    /// <param name="adjusted">If true, returns adjusted weekly data; otherwise, returns unadjusted weekly data. Default is false.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the weekly time series data.</returns>
    public static async Task<JsonDocument> TimeSeriesWeeklyAsync(this IAlphaVantageClient client, string symbol, bool adjusted = false)
        => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_WEEKLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_WEEKLY, symbol);

    /// <summary>
    /// Retrieves the latest global quote information for the specified stock symbol.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the global quote data including price, volume, and trading information.</returns>
    public static async Task<JsonDocument> GlobalQuoteAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.GLOBAL_QUOTE, symbol);

    /// <summary>
    /// Searches for stock symbols matching the specified keywords.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The search keywords to find matching stock symbols.</param>
    /// <returns>A <see cref="Task{JsonDocument}"/> containing the symbol search results with matching stock symbols and company information.</returns>
    public static async Task<JsonDocument> SymbolSearchAsync(this IAlphaVantageClient client, string symbol)
        => await client.GetJsonDocumentAsync(AlphaVantageFunction.SYMBOL_SEARCH, symbol);

    /// <summary>
    /// Retrieves and parses the global quote data for the specified stock symbol into a strongly-typed response.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
    /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="GlobalQuote"/> data or error information.</returns>
    /// <remarks>
    /// This method calls the Alpha Vantage Global Quote API and parses the JSON response into a strongly-typed
    /// <see cref="GlobalQuote"/> object. If the API returns an error or the data cannot be parsed, the response
    /// will contain an error message in the <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<AlphaVantageResponse<GlobalQuote>> GlobalQuote(this IAlphaVantageClient client, string symbol)
    {
        var jsonDocument = await client.GlobalQuoteAsync(symbol);
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty("Global Quote", out JsonElement globalQuoteElement))
        {
            return new AlphaVantageResponse<GlobalQuote>()
            {
                Result = new GlobalQuote
                {
                    Symbol = globalQuoteElement.GetProperty("01. symbol").GetString() ?? string.Empty,
                    Open = decimal.Parse(globalQuoteElement.GetProperty("02. open").GetString() ?? "0"),
                    High = decimal.Parse(globalQuoteElement.GetProperty("03. high").GetString() ?? "0"),
                    Low = decimal.Parse(globalQuoteElement.GetProperty("04. low").GetString() ?? "0"),
                    Price = decimal.Parse(globalQuoteElement.GetProperty("05. price").GetString() ?? "0"),
                    Volume = long.Parse(globalQuoteElement.GetProperty("06. volume").GetString() ?? "0"),
                    LatestTradingDay = DateOnly.Parse(globalQuoteElement.GetProperty("07. latest trading day").GetString() ?? DateTime.MinValue.ToString()),
                    PreviousClose = decimal.Parse(globalQuoteElement.GetProperty("08. previous close").GetString() ?? "0"),
                    Change = decimal.Parse(globalQuoteElement.GetProperty("09. change").GetString() ?? "0")
                }
            };
        }
        var info = root.TryGetProperty("Information", out JsonElement informationElement);
        if (info)
        {
            return new AlphaVantageResponse<GlobalQuote>()
            {
                ErrorMessage = informationElement.GetString() ?? "Global Quote data not found."
            };
        }
        var error = root.TryGetProperty("Error Message", out JsonElement errorElement);
        if (error)
        {
            return new AlphaVantageResponse<GlobalQuote>()
            {
                ErrorMessage = errorElement.GetString() ?? "Global Quote data not found."
            };
        }
        return new AlphaVantageResponse<GlobalQuote>()
        {
            ErrorMessage = "Global Quote data not found."
        };
    }

    /// <summary>
    /// Retrieves global quote data for multiple stock symbols concurrently.
    /// </summary>
    /// <param name="client">The Alpha Vantage client instance.</param>
    /// <param name="symbols">An enumerable collection of stock symbols to retrieve global quotes for.</param>
    /// <returns>A <see cref="Task{IDictionary}"/> containing a dictionary where keys are stock symbols and values are the corresponding <see cref="AlphaVantageResponse{GlobalQuote}"/> data.</returns>
    /// <remarks>
    /// This method executes multiple API calls concurrently to fetch global quote data for all provided symbols.
    /// Each symbol's result is returned as a separate <see cref="AlphaVantageResponse{GlobalQuote}"/> in the dictionary,
    /// allowing individual error handling per symbol. Failed requests will have error information in their respective
    /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
    /// </remarks>
    public static async Task<IDictionary<string, AlphaVantageResponse<GlobalQuote>>> GlobalQuotes(this IAlphaVantageClient client, IEnumerable<string> symbols)
    {
        var tasks = symbols.Select(symbol => client.GlobalQuote(symbol));
        var results = await Task.WhenAll(tasks);
        return results.Select((result, index) => new { Symbol = symbols.ElementAt(index), Result = result })
            .ToDictionary(x => x.Symbol, x => x.Result);
    }

}