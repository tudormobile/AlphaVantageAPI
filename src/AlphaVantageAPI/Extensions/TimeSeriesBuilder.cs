using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal static class TimeSeriesBuilder
{
    internal static TimeSeries? FromDocument(JsonDocument jsonDocument, string symbol, TimeSeries.TimeSeriesInterval interval)
    {
        var root = jsonDocument.RootElement;
        string timeSeriesKey = interval switch
        {
            TimeSeries.TimeSeriesInterval.Daily => "Time Series (Daily)",
            TimeSeries.TimeSeriesInterval.Weekly => "Weekly Time Series",
            TimeSeries.TimeSeriesInterval.Monthly => "Monthly Time Series",
            _ => throw new NotSupportedException("Only Daily, Weekly, and Monthly intervals are supported.")
        };
        if (root.TryGetProperty("Meta Data", out JsonElement metaDataElement))
        {
            var symbolFromMeta = metaDataElement.GetProperty("2. Symbol").GetString();
            if (!string.Equals(symbolFromMeta, symbol, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The symbol '{symbol}' does not match the symbol in the JSON document '{symbolFromMeta}'.", nameof(symbol));
            }

            var lastRefreshed = metaDataElement.GetProperty("3. Last Refreshed").TryGetDateTime(out var lastRefreshedDateTime)
                ? DateOnly.FromDateTime(lastRefreshedDateTime)
                : throw new FormatException("Invalid date format for 'Last Refreshed' in Meta Data.");
            if (root.TryGetProperty(timeSeriesKey, out JsonElement timeSeriesElement))
            {
                var data = new Dictionary<DateOnly, TimeSeriesData>();
                foreach (var property in timeSeriesElement.EnumerateObject())
                {
                    var date = DateOnly.Parse(property.Name);
                    var dataElement = property.Value;
                    var timeSeriesData = new TimeSeriesData
                    {
                        Open = decimal.Parse(dataElement.GetProperty("1. open").GetString() ?? "0"),
                        High = decimal.Parse(dataElement.GetProperty("2. high").GetString() ?? "0"),
                        Low = decimal.Parse(dataElement.GetProperty("3. low").GetString() ?? "0"),
                        Close = decimal.Parse(dataElement.GetProperty("4. close").GetString() ?? "0"),
                        Volume = long.Parse(dataElement.GetProperty("5. volume").GetString() ?? "0")
                    };
                    data[date] = timeSeriesData;
                }
                return new TimeSeries
                {
                    Symbol = symbol,
                    Interval = interval,
                    Data = data,
                    LastUpdated = lastRefreshed,
                };
            }
        }
        return null;
    }
}