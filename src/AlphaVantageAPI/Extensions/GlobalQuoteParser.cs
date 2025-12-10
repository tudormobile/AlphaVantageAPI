using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class GlobalQuoteParser : EntityParser
{
    private const string ROOT_PROPERTY = "Global Quote";
    private const string SYMBOL_PROPERTY = "01. symbol";
    private const string OPEN_PROPERTY = "02. open";
    private const string HIGH_PROPERTY = "03. high";
    private const string LOW_PROPERTY = "04. low";
    private const string PRICE_PROPERTY = "05. price";
    private const string VOLUME_PROPERTY = "06. volume";
    private const string LASTTRADE_PROPERTY = "07. latest trading day";
    private const string PREVCLOSE_PROPERTY = "08. previous close";
    private const string CHANGE_PROPERTY = "09. change";
    private const string CHANGEPCT_PROPERTY = "10. change percent";

    internal static GlobalQuote? FromDocument(JsonDocument jsonDocument, string symbol)
    {
        var root = jsonDocument.RootElement;
        if (root.TryGetProperty(ROOT_PROPERTY, out JsonElement globalQuoteElement))
        {
            return new GlobalQuote
            {
                Symbol = GetStringValue(globalQuoteElement, SYMBOL_PROPERTY),
                Open = GetDecimalValue(globalQuoteElement, OPEN_PROPERTY),
                High = GetDecimalValue(globalQuoteElement, HIGH_PROPERTY),
                Low = GetDecimalValue(globalQuoteElement, LOW_PROPERTY),
                Price = GetDecimalValue(globalQuoteElement, PRICE_PROPERTY),
                Volume = GetLongValue(globalQuoteElement, VOLUME_PROPERTY),
                LatestTradingDay = GetDateOnlyValue(globalQuoteElement, LASTTRADE_PROPERTY),
                PreviousClose = GetDecimalValue(globalQuoteElement, PREVCLOSE_PROPERTY),
                Change = GetDecimalValue(globalQuoteElement, CHANGE_PROPERTY),
            };
        }
        return null;
    }
}

/* Ref: https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=IBM&apikey=demo
{
    "Global Quote": {
        "01. symbol": "IBM",
        "02. open": "309.6300",
        "03. high": "313.9700",
        "04. low": "308.7500",
        "05. price": "310.4800",
        "06. volume": "2914275",
        "07. latest trading day": "2025-12-09",
        "08. previous close": "309.1800",
        "09. change": "1.3000",
        "10. change percent": "0.4205%"
    }
}
*/