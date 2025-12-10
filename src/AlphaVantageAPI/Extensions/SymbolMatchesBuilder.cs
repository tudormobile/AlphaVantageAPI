using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;

namespace Tudormobile.AlphaVantage.Extensions;

internal static class SymbolMatchesBuilder
{
    internal static SymbolMatches FromDocument(JsonDocument jsonDocument, string keywords, SymbolMatch.MatchTypes matchType = SymbolMatch.MatchTypes.Any,
        SymbolMatch.Regions region = SymbolMatch.Regions.Any)
    {
        var matches = new List<SymbolMatch>();
        if (jsonDocument.RootElement.TryGetProperty("bestMatches", out JsonElement bestMatchesElement) && bestMatchesElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var matchElement in bestMatchesElement.EnumerateArray())
            {
                var open = matchElement.GetProperty("5. marketOpen").GetString() ?? "00:00";
                var close = matchElement.GetProperty("6. marketClose").GetString() ?? "00:00";
                var scoreString = matchElement.GetProperty("9. matchScore").GetString() ?? "0";
                var symbolTypeString = matchElement.GetProperty("3. type").GetString() ?? "Unknown";
                var regionString = matchElement.GetProperty("4. region").GetString() ?? "Unknown";

                _ = double.TryParse(scoreString, out double score);
                _ = TimeOnly.TryParse($"{open}:00", out TimeOnly marketOpen);
                _ = TimeOnly.TryParse($"{close}:00", out TimeOnly marketClose);
                _ = Enum.TryParse<SymbolMatch.MatchTypes>(symbolTypeString, true, out SymbolMatch.MatchTypes symbolType);

                var symbolRegion = ParseRegion(regionString);

                if ((matchType == SymbolMatch.MatchTypes.Any || symbolType == matchType)
                    && (region == SymbolMatch.Regions.Any || symbolRegion == region))
                {
                    var symbolMatch = new SymbolMatch
                    {
                        Symbol = matchElement.GetProperty("1. symbol").GetString() ?? String.Empty,
                        Name = matchElement.GetProperty("2. name").GetString() ?? String.Empty,
                        MatchType = symbolType,
                        Region = symbolRegion,
                        RegionName = regionString,
                        MarketOpen = marketOpen,
                        MarketClose = marketClose,
                        Currency = matchElement.GetProperty("8. currency").GetString() ?? String.Empty,
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
