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

    [TestMethod]
    public void FromDocument_WithTypeFilterOnly_ReturnsFilteredMatches()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(mixedTypesJson);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords, SymbolMatch.MatchTypes.ETF);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Matches);
        Assert.AreEqual(SymbolMatch.MatchTypes.ETF, result.Matches[0].MatchType);
    }

    [TestMethod]
    public void FromDocument_WithRegionFilterOnly_ReturnsFilteredMatches()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(json);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords, SymbolMatch.MatchTypes.Any, SymbolMatch.Regions.FFM);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(2, result.Matches);
        foreach (var match in result.Matches)
        {
            Assert.AreEqual(SymbolMatch.Regions.FFM, match.Region);
        }
    }

    [TestMethod]
    public void FromDocument_WithNoMatchingTypeFilter_ReturnsEmptyList()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(json);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords, SymbolMatch.MatchTypes.Bond);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(0, result.Matches);
    }

    [TestMethod]
    public void FromDocument_WithNoMatchingRegionFilter_ReturnsEmptyList()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(json);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords, SymbolMatch.MatchTypes.Any, SymbolMatch.Regions.Unknown);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(0, result.Matches);
    }

    [TestMethod]
    public void FromDocument_WithBestMatchesAsNonArray_ReturnsEmptyList()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(@"{ ""bestMatches"": ""not an array"" }");

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(0, result.Matches);
    }

    [TestMethod]
    public void ParseRegion_WithUnitedStates_ReturnsUS()
    {
        // Act
        var result = SymbolMatchesParser.ParseRegion("United States");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.US, result);
    }

    [TestMethod]
    public void ParseRegion_WithUnitedKingdom_ReturnsUK()
    {
        // Act
        var result = SymbolMatchesParser.ParseRegion("United Kingdom");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.UK, result);
    }

    [TestMethod]
    public void ParseRegion_WithFrankfurt_ReturnsFFM()
    {
        // Act
        var result = SymbolMatchesParser.ParseRegion("Frankfurt");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.FFM, result);
    }

    [TestMethod]
    public void ParseRegion_WithEnumNameCaseInsensitive_ReturnsEnum()
    {
        // Act
        var resultLower = SymbolMatchesParser.ParseRegion("us");
        var resultUpper = SymbolMatchesParser.ParseRegion("US");
        var resultMixed = SymbolMatchesParser.ParseRegion("Us");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.US, resultLower);
        Assert.AreEqual(SymbolMatch.Regions.US, resultUpper);
        Assert.AreEqual(SymbolMatch.Regions.US, resultMixed);
    }

    [TestMethod]
    public void ParseRegion_WithUnknownRegion_ReturnsUnknown()
    {
        // Act
        var result = SymbolMatchesParser.ParseRegion("Mars");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.Unknown, result);
    }

    [TestMethod]
    public void ParseRegion_WithEmptyString_ReturnsUnknown()
    {
        // Act
        var result = SymbolMatchesParser.ParseRegion("");

        // Assert
        Assert.AreEqual(SymbolMatch.Regions.Unknown, result);
    }

    [TestMethod]
    public void FromDocument_WithAllMatchTypes_ParsesCorrectly()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(allTypesJson);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(8, result.Matches);
        Assert.AreEqual(SymbolMatch.MatchTypes.Equity, result.Matches[0].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.ETF, result.Matches[1].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.MutualFund, result.Matches[2].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.Index, result.Matches[3].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.Commodity, result.Matches[4].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.Currency, result.Matches[5].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.Cryptocurrency, result.Matches[6].MatchType);
        Assert.AreEqual(SymbolMatch.MatchTypes.Bond, result.Matches[7].MatchType);
    }

    [TestMethod]
    public void FromDocument_WithMissingOptionalFields_UsesDefaults()
    {
        // Arrange
        var keywords = "Test";
        var doc = JsonDocument.Parse(minimalJson);

        // Act
        var result = SymbolMatchesParser.FromDocument(doc, keywords);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Matches);
        var match = result.Matches[0];
        Assert.AreEqual("TEST", match.Symbol);
        Assert.AreEqual("", match.Name);
        Assert.AreEqual("", match.Currency);
        Assert.AreEqual(0.0, match.MatchScore);
        Assert.AreEqual(TimeOnly.MinValue, match.MarketOpen);
        Assert.AreEqual(TimeOnly.MinValue, match.MarketClose);
    }


    private readonly string invalidJson = @"
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

    private readonly string json = @"
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

    private readonly string mixedTypesJson = @"
{
    ""bestMatches"": [
        {
            ""1. symbol"": ""SPY"",
            ""2. name"": ""SPDR S&P 500 ETF"",
            ""3. type"": ""ETF"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""7. timezone"": ""UTC-04"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""AAPL"",
            ""2. name"": ""Apple Inc"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""7. timezone"": ""UTC-04"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.8""
        }
    ]
}";

    private readonly string allTypesJson = @"
{
    ""bestMatches"": [
        {
            ""1. symbol"": ""AAPL"",
            ""2. name"": ""Apple Inc"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""SPY"",
            ""2. name"": ""SPDR S&P 500"",
            ""3. type"": ""ETF"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""VFIAX"",
            ""2. name"": ""Vanguard 500 Index"",
            ""3. type"": ""MutualFund"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""SPX"",
            ""2. name"": ""S&P 500 Index"",
            ""3. type"": ""Index"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""GC"",
            ""2. name"": ""Gold Futures"",
            ""3. type"": ""Commodity"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""EUR/USD"",
            ""2. name"": ""Euro US Dollar"",
            ""3. type"": ""Currency"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""BTC"",
            ""2. name"": ""Bitcoin"",
            ""3. type"": ""Cryptocurrency"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""00:00"",
            ""6. marketClose"": ""23:59"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        },
        {
            ""1. symbol"": ""US10Y"",
            ""2. name"": ""US 10 Year Treasury"",
            ""3. type"": ""Bond"",
            ""4. region"": ""United States"",
            ""5. marketOpen"": ""09:30"",
            ""6. marketClose"": ""16:00"",
            ""8. currency"": ""USD"",
            ""9. matchScore"": ""0.9""
        }
    ]
}";

    private readonly string minimalJson = @"
{
    ""bestMatches"": [
        {
            ""1. symbol"": ""TEST"",
            ""3. type"": ""Equity"",
            ""4. region"": ""United States""
        }
    ]
}";
}
