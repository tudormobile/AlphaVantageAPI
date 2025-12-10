using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class TimeSeriesParser : EntityParser
{
    private const string META_DATA_PROPERTY = "Meta Data";
    private const string DAILY_ROOT_PROPERTY = "Time Series (Daily)";
    private const string WEEKLY_ROOT_PROPERTY = "Weekly Time Series";
    private const string MONTHLY_ROOT_PROPERTY = "Monthly Time Series";

    private const string SYMBOL_PROPERTY = "2. Symbol";
    private const string LAST_REFRESHED_PROPERTY = "3. Last Refreshed";

    private const string OPEN_PROPERTY = "1. open";
    private const string HIGH_PROPERTY = "2. high";
    private const string LOW_PROPERTY = "3. low";
    private const string CLOSE_PROPERTY = "4. close";
    private const string VOLUME_PROPERTY = "5. volume";

    internal static TimeSeries? FromDocument(JsonDocument jsonDocument, string symbol, TimeSeries.TimeSeriesInterval interval)
    {
        var root = jsonDocument.RootElement;
        string timeSeriesKey = interval switch
        {
            TimeSeries.TimeSeriesInterval.Daily => DAILY_ROOT_PROPERTY,
            TimeSeries.TimeSeriesInterval.Weekly => WEEKLY_ROOT_PROPERTY,
            TimeSeries.TimeSeriesInterval.Monthly => MONTHLY_ROOT_PROPERTY,
            _ => throw new NotSupportedException("Only Daily, Weekly, and Monthly intervals are supported.")
        };
        if (root.TryGetProperty(META_DATA_PROPERTY, out JsonElement metaDataElement))
        {
            var symbolFromMeta = GetStringValue(metaDataElement, SYMBOL_PROPERTY);
            if (!string.Equals(symbolFromMeta, symbol, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"The symbol '{symbol}' does not match the symbol in the JSON document '{symbolFromMeta}'.", nameof(symbol));
            }

            var lastRefreshed = GetDateOnlyValue(metaDataElement, LAST_REFRESHED_PROPERTY);
            if (lastRefreshed == default)
            {
                throw new FormatException("The 'Last Refreshed' date is missing or invalid in the JSON document.");
            }
            if (root.TryGetProperty(timeSeriesKey, out JsonElement timeSeriesElement))
            {
                var data = new Dictionary<DateOnly, TimeSeriesData>();
                foreach (var property in timeSeriesElement.EnumerateObject())
                {
                    if (DateOnly.TryParse(property.Name, out var date))
                    {
                        var dataElement = property.Value;
                        var timeSeriesData = new TimeSeriesData
                        {
                            Open = GetDecimalValue(dataElement, OPEN_PROPERTY),
                            High = GetDecimalValue(dataElement, HIGH_PROPERTY),
                            Low = GetDecimalValue(dataElement, LOW_PROPERTY),
                            Close = GetDecimalValue(dataElement, CLOSE_PROPERTY),
                            Volume = GetLongValue(dataElement, VOLUME_PROPERTY)
                        };
                        data[date] = timeSeriesData;
                    }
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

/* Ref: https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo
{
    "Meta Data": {
        "1. Information": "Daily Prices (open, high, low, close) and Volumes",
        "2. Symbol": "IBM",
        "3. Last Refreshed": "2025-12-09",
        "4. Output Size": "Compact",
        "5. Time Zone": "US/Eastern"
    },
    "Time Series (Daily)": {
        "2025-12-09": {
            "1. open": "309.6300",
            "2. high": "313.9700",
            "3. low": "308.7500",
            "4. close": "310.4800",
            "5. volume": "2914275"
        },
        ...
    }
}
*/