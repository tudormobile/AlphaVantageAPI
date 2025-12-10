using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class SymbolMatchesParserTests
{
    [TestMethod]
    public void FromDocumentTest()
    {
        var keywords = "Apple";
        var doc = JsonDocument.Parse(json);
        var result = SymbolMatchesParser.FromDocument(doc, keywords);

        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(5, result.Matches);

        var firstMatch = result.Matches[0];

        Assert.AreEqual("TSCO.LON", firstMatch.Symbol);
        Assert.AreEqual("Tesco PLC", firstMatch.Name);
        Assert.AreEqual(SymbolMatch.MatchTypes.Equity, firstMatch.MatchType);
        Assert.AreEqual("United Kingdom", firstMatch.RegionName);
        Assert.AreEqual(SymbolMatch.Regions.UK, firstMatch.Region);
        Assert.AreEqual(new TimeOnly(8, 0), firstMatch.MarketOpen);
        Assert.AreEqual(new TimeOnly(16, 30), firstMatch.MarketClose);
        Assert.AreEqual("GBX", firstMatch.Currency);
        Assert.AreEqual(0.7273, firstMatch.MatchScore);
    }

    [TestMethod]
    public void FromDocumentWithRegionAndTypeFilterTest()
    {
        var keywords = "Tesco";
        var doc = JsonDocument.Parse(json);
        var result = SymbolMatchesParser.FromDocument(doc, keywords, SymbolMatch.MatchTypes.Equity, SymbolMatch.Regions.US);
        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(2, result.Matches);
        foreach (var match in result.Matches)
        {
            Assert.AreEqual(SymbolMatch.MatchTypes.Equity, match.MatchType);
            Assert.AreEqual(SymbolMatch.Regions.US, match.Region);
        }
    }

    [TestMethod]
    public void FromDocumentWithNoMatchesTest()
    {
        var keywords = "Tesco";
        var doc = JsonDocument.Parse(@"{ ""bestMatches"": [] }");
        var result = SymbolMatchesParser.FromDocument(doc, keywords);
        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(0, result.Matches);
    }

    [TestMethod]
    public void FromDocumentWithEmptyDocumentTest()
    {
        var keywords = "Tesco";
        var doc = JsonDocument.Parse(@"{}");
        var result = SymbolMatchesParser.FromDocument(doc, keywords);
        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(0, result.Matches);
    }

    [TestMethod]
    public void FromDocumentWithInvalidDataTest()
    {
        var keywords = "Tesco";
        var doc = JsonDocument.Parse(invalidJson);
        var result = SymbolMatchesParser.FromDocument(doc, keywords);
        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(1, result.Matches);

        var firstMatch = result.Matches[0];
        Assert.AreEqual("TSCO.LON", firstMatch.Symbol);
        Assert.AreEqual("Tesco PLC", firstMatch.Name);
        Assert.AreEqual(SymbolMatch.MatchTypes.Unknown, firstMatch.MatchType);
        Assert.AreEqual(SymbolMatch.Regions.Unknown, firstMatch.Region);
    }

    [TestMethod]
    public void FromDocumentWithNullDataTest()
    {
        var keywords = "Tesco";
        var doc = JsonDocument.Parse(nullJson);
        var result = SymbolMatchesParser.FromDocument(doc, keywords);
        Assert.IsNotNull(result);
        Assert.AreEqual(keywords, result.Keywords);
        Assert.HasCount(1, result.Matches);

        var firstMatch = result.Matches[0];
        Assert.AreEqual("", firstMatch.Symbol);
        Assert.AreEqual("", firstMatch.Name);
        Assert.AreEqual(SymbolMatch.MatchTypes.Unknown, firstMatch.MatchType);
        Assert.AreEqual(SymbolMatch.Regions.Unknown, firstMatch.Region);
    }


    private string invalidJson = @"
{
    ""bestMatches"": [
        {
            ""1. symbol"": ""TSCO.LON"",
            ""2. name"": ""Tesco PLC"",
            ""3. type"": ""bad-type"",
            ""4. region"": ""bad-region"",
            ""5. marketOpen"": ""bad-open"",
            ""6. marketClose"": ""bad-close"",
            ""7. timezone"": ""bad-timezone"",
            ""8. currency"": ""bad-currency"",
            ""9. matchScore"": ""bad-score""
        }
    ]
}
";

    private string nullJson = @"
{
    ""bestMatches"": [
        {
            ""1. symbol"": null,
            ""2. name"": null,
            ""3. type"": null,
            ""4. region"": null,
            ""5. marketOpen"": null,
            ""6. marketClose"": null,
            ""7. timezone"": null,
            ""8. currency"": null,
            ""9. matchScore"": null
        }
    ]
}
";

    private string json = @"
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
        },
        {
            ""1. symbol"": ""TSCDF"",
            ""2. name"": ""Tesco plc"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""7. timezone"": ""UTC-04"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.7143""
        },
        {
            ""1. symbol"": ""TSCDY"",
            ""2. name"": ""Tesco plc"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""7. timezone"": ""UTC-04"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.7143""
        },
        {
            ""1. symbol"": ""TCO2.FRK"",
            ""2. name"": ""TESCO PLC ADR/1 LS-05"",
            ""3. type"": ""Equity"",
            ""4. region"": ""Frankfurt"",
            ""5. marketOpen"": ""08:00"",
            ""6. marketClose"": ""20:00"",
            ""7. timezone"": ""UTC+02"",
            ""8. currency"": ""EUR"",
            ""9. matchScore"": ""0.5455""
        },
        {
            ""1. symbol"": ""TCO0.FRK"",
            ""2. name"": ""TESCO PLC LS-0633333"",
            ""3. type"": ""Equity"",
            ""4. region"": ""Frankfurt"",
            ""5. marketOpen"": ""08:00"",
            ""6. marketClose"": ""20:00"",
            ""7. timezone"": ""UTC+02"",
            ""8. currency"": ""EUR"",
            ""9. matchScore"": ""0.5455""
        }
    ]
}";

}
