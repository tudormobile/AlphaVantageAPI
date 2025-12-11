using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

/// <summary>
/// Provides extension methods for working with instances of <see cref="IAlphaVantageClient"/>.
/// </summary>
/// <remarks>
/// This class contains static methods that extend the functionality of <see cref="IAlphaVantageClient"/>
/// implementations, enabling fluent configuration and builder patterns. These extensions are intended to simplify
/// client setup and integration scenarios.
/// <para>
/// The code uses the newer C# extension method syntax with the 'extension' keyword for better readability and conciseness.
/// </para>
/// </remarks>
public static class AlphaVantageClientExtensions
{
    // Extension block - receiver type only. These methods appear as static methods on IAlphaVantageClient.
    extension(IAlphaVantageClient)
    {
        /// <summary>
        /// Creates a new builder for configuring and constructing an instance of an Alpha Vantage client.
        /// </summary>
        /// <returns>An <see cref="IBuilder{IAlphaVantageClient}"/> that can be used to configure and build an <see
        /// cref="IAlphaVantageClient"/> instance.</returns>
        public static IAlphaVantageClientBuilder GetBuilder() => new AlphaVantageClientBuilder();
    }

    // Extension block - receiver type and instance parameter. These methods appear as instance methods on IAlphaVantageClient.
    extension(IAlphaVantageClient client)
    {

        /// <summary>
        /// Retrieves daily time series data for the specified stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve daily time series data for.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{JsonDocument}"/> containing the daily time series data.</returns>
        public async Task<JsonDocument> TimeSeriesDailyJsonAsync(string symbol, CancellationToken cancellationToken = default)
            => await client.GetJsonDocumentAsync(AlphaVantageFunction.TIME_SERIES_DAILY, symbol, cancellationToken);

        /// <summary>
        /// Retrieves monthly time series data for the specified stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve monthly time series data for.</param>
        /// <param name="adjusted">If true, returns adjusted monthly data; otherwise, returns unadjusted monthly data. Default is false.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{JsonDocument}"/> containing the monthly time series data.</returns>
        public async Task<JsonDocument> TimeSeriesMonthlyJsonAsync(string symbol, bool adjusted = false, CancellationToken cancellationToken = default)
            => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_MONTHLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_MONTHLY, symbol, cancellationToken);

        /// <summary>
        /// Retrieves weekly time series data for the specified stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve weekly time series data for.</param>
        /// <param name="adjusted">If true, returns adjusted weekly data; otherwise, returns unadjusted weekly data. Default is false.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{JsonDocument}"/> containing the weekly time series data.</returns>
        public async Task<JsonDocument> TimeSeriesWeeklyJsonAsync(string symbol, bool adjusted = false, CancellationToken cancellationToken = default)
            => await client.GetJsonDocumentAsync(adjusted ? AlphaVantageFunction.TIME_SERIES_WEEKLY_ADJUSTED : AlphaVantageFunction.TIME_SERIES_WEEKLY, symbol, cancellationToken);

        /// <summary>
        /// Retrieves the latest global quote information for the specified stock symbol.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{JsonDocument}"/> containing the global quote data including price, volume, and trading information.</returns>
        public async Task<JsonDocument> GlobalQuoteJsonAsync(string symbol, CancellationToken cancellationToken = default)
            => await client.GetJsonDocumentAsync(AlphaVantageFunction.GLOBAL_QUOTE, symbol, cancellationToken);

        /// <summary>
        /// Searches for stock symbols matching the specified keywords.
        /// </summary>
        /// <param name="symbol">The search keywords to find matching stock symbols.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{JsonDocument}"/> containing the symbol search results with matching stock symbols and company information.</returns>
        public async Task<JsonDocument> SymbolSearchJsonAsync(string symbol, CancellationToken cancellationToken = default)
            => await client.GetJsonDocumentAsync(AlphaVantageFunction.SYMBOL_SEARCH, symbol, cancellationToken);

        /// <summary>
        /// Retrieves and parses the global quote data for the specified stock symbol into a strongly-typed response.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve the global quote for.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="GetGlobalQuoteAsync"/> data or error information.</returns>
        /// <remarks>
        /// This method calls the Alpha Vantage Global Quote API and parses the JSON response into a strongly-typed
        /// <see cref="GetGlobalQuoteAsync"/> object. If the API returns an error or the data cannot be parsed, the response
        /// will contain an error message in the <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
        /// </remarks>
        public async Task<AlphaVantageResponse<GlobalQuote>> GetGlobalQuoteAsync(string symbol, CancellationToken cancellationToken = default)
        {
            using var jsonDocument = await client.GlobalQuoteJsonAsync(symbol, cancellationToken);
            var result = GlobalQuoteParser.FromDocument(jsonDocument, symbol);
            return ResponseParser.CreateResponse(result, jsonDocument, "Global quote data not found.");
        }

        /// <summary>
        /// Retrieves global quote data for multiple stock symbols concurrently.
        /// </summary>
        /// <param name="symbols">An enumerable collection of stock symbols to retrieve global quotes for.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{IDictionary}"/> containing a dictionary where keys are stock symbols and values are the corresponding <see cref="AlphaVantageResponse{GlobalQuote}"/> data.</returns>
        /// <remarks>
        /// This method executes multiple API calls concurrently to fetch global quote data for all provided symbols.
        /// Each symbol's result is returned as a separate <see cref="AlphaVantageResponse{GlobalQuote}"/> in the dictionary,
        /// allowing individual error handling per symbol. Failed requests will have error information in their respective
        /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
        /// <para>
        /// <strong>Warning:</strong> This method does not implement rate limiting. If your Alpha Vantage API key has rate limits,
        /// you are responsible for throttling calls. Consider using Polly or a similar library to implement rate limiting at the application level.
        /// </para>
        /// </remarks>
        public async Task<IDictionary<string, AlphaVantageResponse<GlobalQuote>>> GetGlobalQuotesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default)
        {
            var symbolsList = symbols.ToList();
            var tasks = symbolsList.Select(symbol => client.GetGlobalQuoteAsync(symbol, cancellationToken));
            var results = await Task.WhenAll(tasks);
            return symbolsList.Zip(results, (symbol, result) => new { symbol, result })
                .ToDictionary(x => x.symbol, x => x.result);
        }

        /// <summary>
        /// Retrieves and parses daily time series data for the specified stock symbol into a strongly-typed response.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve daily time series data for.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with daily interval or error information.</returns>
        /// <remarks>
        /// This method calls the Alpha Vantage Daily Time Series API and parses the JSON response into a strongly-typed
        /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Daily"/> interval. If the API
        /// returns an error or the data cannot be parsed, the response will contain an error message in the
        /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
        /// </remarks>
        public async Task<AlphaVantageResponse<TimeSeries>> GetDailyTimeSeriesAsync(string symbol, CancellationToken cancellationToken = default)
        {
            using var jsonDoc = await client.TimeSeriesDailyJsonAsync(symbol, cancellationToken);
            return TimeSeriesResult(jsonDoc, symbol, TimeSeries.TimeSeriesInterval.Daily);
        }

        /// <summary>
        /// Retrieves and parses weekly time series data for the specified stock symbol into a strongly-typed response.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve weekly time series data for.</param>
        /// <param name="adjusted">If true, returns adjusted weekly data that accounts for stock splits and dividends; otherwise, returns unadjusted weekly data. Default is false.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with weekly interval or error information.</returns>
        /// <remarks>
        /// This method calls the Alpha Vantage Weekly Time Series API and parses the JSON response into a strongly-typed
        /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Weekly"/> interval. If the API
        /// returns an error or the data cannot be parsed, the response will contain an error message in the
        /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
        /// </remarks>
        public async Task<AlphaVantageResponse<TimeSeries>> GetWeeklyTimeSeriesAsync(string symbol, bool adjusted = false, CancellationToken cancellationToken = default)
        {
            using var jsonDoc = await client.TimeSeriesWeeklyJsonAsync(symbol, adjusted, cancellationToken);
            return TimeSeriesResult(jsonDoc, symbol, TimeSeries.TimeSeriesInterval.Weekly);
        }

        /// <summary>
        /// Retrieves and parses monthly time series data for the specified stock symbol into a strongly-typed response.
        /// </summary>
        /// <param name="symbol">The stock symbol to retrieve monthly time series data for.</param>
        /// <param name="adjusted">If true, returns adjusted monthly data that accounts for stock splits and dividends; otherwise, returns unadjusted monthly data. Default is false.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{AlphaVantageResponse}"/> containing either the parsed <see cref="TimeSeries"/> data with monthly interval or error information.</returns>
        /// <remarks>
        /// This method calls the Alpha Vantage Monthly Time Series API and parses the JSON response into a strongly-typed
        /// <see cref="TimeSeries"/> object with <see cref="TimeSeries.TimeSeriesInterval.Monthly"/> interval. If the API
        /// returns an error or the data cannot be parsed, the response will contain an error message in the
        /// <see cref="AlphaVantageResponse{T}.ErrorMessage"/> property.
        /// </remarks>
        public async Task<AlphaVantageResponse<TimeSeries>> GetMonthlyTimeSeriesAsync(string symbol, bool adjusted = false, CancellationToken cancellationToken = default)
        {
            using var jsonDocument = await client.TimeSeriesMonthlyJsonAsync(symbol, adjusted, cancellationToken);
            return TimeSeriesResult(jsonDocument, symbol, TimeSeries.TimeSeriesInterval.Monthly);
        }
        /// <summary>
        /// Searches for financial symbols that match the specified keywords using the Alpha Vantage API.
        /// </summary>
        /// <remarks>The search results may vary depending on the keywords, match type, and region specified. If
        /// no matching symbols are found, the response will contain an error message. This method is asynchronous and
        /// should be awaited.</remarks>
        /// <param name="keywords">The keywords to search for. Typically a company name, ticker symbol, or related term. Cannot be null or empty.</param>
        /// <param name="matchType">The type of match to perform when searching for symbols. Specifies whether to match any, exact, or partial
        /// results. Defaults to <see cref="SymbolMatch.MatchTypes.Any"/>.</param>
        /// <param name="region">The region to filter symbol search results by. Specifies the geographic area for the search. Defaults to <see
        /// cref="SymbolMatch.Regions.Any"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="AlphaVantageResponse{SymbolMatches}"/> with the matching symbols, or an error message if no results are
        /// found.</returns>
        public async Task<AlphaVantageResponse<SymbolMatches>> SymbolSearchAsync(
            string keywords,
            SymbolMatch.MatchTypes matchType = SymbolMatch.MatchTypes.Any,
            SymbolMatch.Regions region = SymbolMatch.Regions.Any,
            CancellationToken cancellationToken = default)
        {
            using var jsonDocument = await client.SymbolSearchJsonAsync(keywords, cancellationToken);
            var result = SymbolMatchesParser.FromDocument(jsonDocument, keywords, matchType, region);
            return ResponseParser.CreateResponse(result, jsonDocument, "Symbol search data not available.");
        }
    }

    private static AlphaVantageResponse<TimeSeries> TimeSeriesResult(JsonDocument jsonDocument, string symbol, TimeSeries.TimeSeriesInterval interval)
    {
        var result = TimeSeriesParser.FromDocument(jsonDocument, symbol, interval);
        return ResponseParser.CreateResponse(result, jsonDocument, "Time series data not found.");
    }
}
