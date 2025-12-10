using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal class SymbolMatchesParser : EntityParser
{
    private const string ROOT_PROPERTY = "bestMatches";

    private const string SYMBOL_PROPERTY = "1. symbol";
    private const string NAME_PROPERTY = "2. name";
    private const string TYPE_PROPERTY = "3. type";
    private const string REGION_PROPERTY = "4. region";
    private const string MARKET_OPEN_PROPERTY = "5. marketOpen";
    private const string MARKET_CLOSE_PROPERTY = "6. marketClose";
    private const string CURRENCY_PROPERTY = "8. currency";
    private const string MATCH_SCORE_PROPERTY = "9. matchScore";

    internal static SymbolMatches FromDocument(JsonDocument jsonDocument, string keywords, SymbolMatch.MatchTypes matchType = SymbolMatch.MatchTypes.Any,
        SymbolMatch.Regions region = SymbolMatch.Regions.Any)
    {
        var matches = new List<SymbolMatch>();
        if (jsonDocument.RootElement.TryGetProperty(ROOT_PROPERTY, out JsonElement rootElement) && rootElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var matchElement in rootElement.EnumerateArray())
            {
                var marketOpen = GetTimeOnlyValue(matchElement, MARKET_OPEN_PROPERTY);
                var marketClose = GetTimeOnlyValue(matchElement, MARKET_CLOSE_PROPERTY);
                var score = GetDoubleValue(matchElement, MATCH_SCORE_PROPERTY);
                var symbolType = GetEnumValue(matchElement, TYPE_PROPERTY, SymbolMatch.MatchTypes.Unknown);

                var regionString = GetStringValue(matchElement, REGION_PROPERTY, "Unknown");
                var symbolRegion = ParseRegion(regionString);

                if ((matchType == SymbolMatch.MatchTypes.Any || symbolType == matchType)
                    && (region == SymbolMatch.Regions.Any || symbolRegion == region))
                {
                    var symbolMatch = new SymbolMatch
                    {
                        Symbol = GetStringValue(matchElement, SYMBOL_PROPERTY),
                        Name = GetStringValue(matchElement, NAME_PROPERTY),
                        MatchType = symbolType,
                        Region = symbolRegion,
                        RegionName = regionString,
                        MarketOpen = marketOpen,
                        MarketClose = marketClose,
                        Currency = GetStringValue(matchElement, CURRENCY_PROPERTY),
                        MatchScore = score
                    };
                    matches.Add(symbolMatch);
                }
            }
        }
        return new SymbolMatches
        {
            Keywords = keywords,
            Matches = matches
        };
    }

    internal static SymbolMatch.Regions ParseRegion(string regionString)
    {
        var result = regionString switch
        {
            "United States" => SymbolMatch.Regions.US,
            "United Kingdom" => SymbolMatch.Regions.UK,
            "Frankfurt" => SymbolMatch.Regions.FFM,
            _ => SymbolMatch.Regions.Unknown
        };
        if (result != SymbolMatch.Regions.Unknown)
        {
            return result;
        }

        _ = Enum.TryParse<SymbolMatch.Regions>(regionString, true, out SymbolMatch.Regions region);
        return region;
    }
}

// Ref: 
/*
{
    ""bestMatches"": [
        {
            ""1. symbol"": ""TSCO.LON"",
            ""2. name"": ""Tesco PLC"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United Kingdom"",
            ""5. marketOpen"": ""08:00"",
            ""6. marketClose"": ""16:30"",
            ""7. timezone"": ""UTC+01"",
            ""8. currency"": ""GBX"",
            ""9. matchScore"": ""0.7273""
        }
    ]
}
*/
