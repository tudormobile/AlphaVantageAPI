using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal static class GlobalQuoteBuilder
{
    internal static GlobalQuote? FromDocument(JsonDocument jsonDocument, string symbol)
    {
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty("Global Quote", out JsonElement globalQuoteElement))
        {
            return new GlobalQuote
            {
                Symbol = globalQuoteElement.GetProperty("01. symbol").GetString() ?? string.Empty,
                Open = decimal.Parse(globalQuoteElement.GetProperty("02. open").GetString() ?? "0"),
                High = decimal.Parse(globalQuoteElement.GetProperty("03. high").GetString() ?? "0"),
                Low = decimal.Parse(globalQuoteElement.GetProperty("04. low").GetString() ?? "0"),
                Price = decimal.Parse(globalQuoteElement.GetProperty("05. price").GetString() ?? "0"),
                Volume = long.Parse(globalQuoteElement.GetProperty("06. volume").GetString() ?? "0"),
                LatestTradingDay = DateOnly.Parse(globalQuoteElement.GetProperty("07. latest trading day").GetString() ?? DateOnly.MinValue.ToString()),
                PreviousClose = decimal.Parse(globalQuoteElement.GetProperty("08. previous close").GetString() ?? "0"),
                Change = decimal.Parse(globalQuoteElement.GetProperty("09. change").GetString() ?? "0")
            };
        }
        return null;
    }
}