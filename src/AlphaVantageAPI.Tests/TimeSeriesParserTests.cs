using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class TimeSeriesParserTests
{
    readonly string jsonDaily = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {
        ""2025-12-08"": {
            ""1. open"": ""309.6200"",
            ""2. high"": ""315.3454"",
            ""3. low"": ""307.9500"",
            ""4. close"": ""309.1800"",
            ""5. volume"": ""3615794""
        },
        ""2025-12-05"": {
            ""1. open"": ""308.5900"",
            ""2. high"": ""311.8300"",
            ""3. low"": ""307.1800"",
            ""4. close"": ""307.9400"",
            ""5. volume"": ""2344667""
        }
    }
}";
    readonly string jsonWeekly = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Weekly Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Time Zone"": ""US/Eastern""
    },
    ""Weekly Time Series"": {
        ""2025-12-08"": {
            ""1. open"": ""309.6200"",
            ""2. high"": ""315.3454"",
            ""3. low"": ""307.9500"",
            ""4. close"": ""309.1800"",
            ""5. volume"": ""3615794""
        },
        ""2025-12-05"": {
            ""1. open"": ""306.5050"",
            ""2. high"": ""311.8300"",
            ""3. low"": ""298.9050"",
            ""4. close"": ""307.9400"",
            ""5. volume"": ""16688175""
        },
        ""2025-11-28"": {
            ""1. open"": ""299.1800"",
            ""2. high"": ""309.1800"",
            ""3. low"": ""297.0600"",
            ""4. close"": ""308.5800"",
            ""5. volume"": ""12761439""
        }
    }
}";

    readonly string jsonMonthly = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Monthly Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Time Zone"": ""US/Eastern""
    },
    ""Monthly Time Series"": {
        ""2025-12-08"": {
            ""1. open"": ""306.5050"",
            ""2. high"": ""315.3454"",
            ""3. low"": ""298.9050"",
            ""4. close"": ""309.1800"",
            ""5. volume"": ""20303969""
        },
        ""2025-11-28"": {
            ""1. open"": ""308.0000"",
            ""2. high"": ""324.9000"",
            ""3. low"": ""288.0700"",
            ""4. close"": ""308.5800"",
            ""5. volume"": ""85897120""
        }
    }
}";

    [TestMethod]
    public void TestBuildDailyTimeSeries()
    {
        var timeSeries = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonDaily), "IBM", TimeSeries.TimeSeriesInterval.Daily);

        Assert.IsNotNull(timeSeries);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Daily, timeSeries.Interval);
        Assert.AreEqual("IBM", timeSeries.Symbol);
        Assert.AreEqual(new DateOnly(2025, 12, 8), timeSeries.LastUpdated);
        Assert.HasCount(2, timeSeries.Data);

        var entry1 = timeSeries.Data!.First();
        Assert.AreEqual(new DateOnly(2025, 12, 8), entry1.Key);
        Assert.AreEqual(309.62m, entry1.Value.Open);
        Assert.AreEqual(315.3454m, entry1.Value.High);
        Assert.AreEqual(307.95m, entry1.Value.Low);
        Assert.AreEqual(309.18m, entry1.Value.Close);
        Assert.AreEqual(3615794, entry1.Value.Volume);

        var entry2 = timeSeries.Data!.Skip(1).First()!;
        Assert.AreEqual(new DateOnly(2025, 12, 5), entry2.Key);
        Assert.AreEqual(308.59m, entry2.Value.Open);
        Assert.AreEqual(311.83m, entry2.Value.High);
        Assert.AreEqual(307.18m, entry2.Value.Low);
        Assert.AreEqual(307.94m, entry2.Value.Close);
        Assert.AreEqual(2344667, entry2.Value.Volume);
    }

    [TestMethod]
    public void TestBuildWeeklyTimeSeries()
    {
        var timeSeries = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonWeekly), "IBM", TimeSeries.TimeSeriesInterval.Weekly);
        Assert.IsNotNull(timeSeries);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Weekly, timeSeries.Interval);
        Assert.AreEqual("IBM", timeSeries.Symbol);
        Assert.AreEqual(new DateOnly(2025, 12, 8), timeSeries.LastUpdated);
        Assert.HasCount(3, timeSeries.Data);

        var entry1 = timeSeries.Data!.First();
        Assert.AreEqual(new DateOnly(2025, 12, 8), entry1.Key);
        Assert.AreEqual(309.62m, entry1.Value.Open);
        Assert.AreEqual(315.3454m, entry1.Value.High);
        Assert.AreEqual(307.95m, entry1.Value.Low);
        Assert.AreEqual(309.18m, entry1.Value.Close);
        Assert.AreEqual(3615794, entry1.Value.Volume);
    }

    [TestMethod]
    public void TestMonthlyTimeSeries()
    {
        var timeSeries = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonMonthly), "IBM", TimeSeries.TimeSeriesInterval.Monthly);
        Assert.IsNotNull(timeSeries);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Monthly, timeSeries.Interval);
        Assert.AreEqual("IBM", timeSeries.Symbol);
        Assert.AreEqual(new DateOnly(2025, 12, 8), timeSeries.LastUpdated);
        Assert.HasCount(2, timeSeries.Data);
        var entry1 = timeSeries.Data!.First();
        Assert.AreEqual(new DateOnly(2025, 12, 8), entry1.Key);
        Assert.AreEqual(306.5050m, entry1.Value.Open);
        Assert.AreEqual(315.3454m, entry1.Value.High);
        Assert.AreEqual(298.9050m, entry1.Value.Low);
        Assert.AreEqual(309.18m, entry1.Value.Close);
        Assert.AreEqual(20303969, entry1.Value.Volume);
    }

    [TestMethod]
    public void TestFromDocumentWithEmptyDocument()
    {
        var document = JsonDocument.Parse("{}");
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);
        Assert.IsNull(timeSeries);
    }

    [TestMethod]
    public void TestFromDocumentWithMismatchedSymbol()
    {
        var document = JsonDocument.Parse(jsonDaily);
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "AAPL", TimeSeries.TimeSeriesInterval.Daily);
        });
    }

    [TestMethod]
    public void TestFromDocumentWithInvalidDateFormat()
    {
        var invalidJson = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""Invalid-Date-Format"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {}
}";
        var document = JsonDocument.Parse(invalidJson);
        Assert.ThrowsExactly<FormatException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);
        });
    }

    [TestMethod]
    public void TestFromDocumentWithUnsupportedInterval()
    {
        var document = JsonDocument.Parse(jsonDaily);
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", (TimeSeries.TimeSeriesInterval)999);
        });
    }

    [TestMethod]
    public void TestFromDocumentWithBadTimeSeriesData()
    {
        var badJson = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {
        ""2025-12-08"": {
            ""1. open"": null,
            ""2. high"": null,
            ""3. low"": null,
            ""4. close"": null,
            ""5. volume"": null
        }
    }
}";
        var document = JsonDocument.Parse(badJson);
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);
        Assert.IsNotNull(timeSeries);
        Assert.HasCount(1, timeSeries.Data);
        var entry = timeSeries.Data!.First();
        Assert.AreEqual(new DateOnly(2025, 12, 8), entry.Key);
        Assert.AreEqual(0m, entry.Value.Open);
        Assert.AreEqual(0m, entry.Value.High);
        Assert.AreEqual(0m, entry.Value.Low);
        Assert.AreEqual(0m, entry.Value.Close);
        Assert.AreEqual(0L, entry.Value.Volume);
    }

    [TestMethod]
    public void FromDocument_WithCaseInsensitiveSymbol_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act - symbol case doesn't match metadata
        var timeSeries = TimeSeriesParser.FromDocument(document, "ibm", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNotNull(timeSeries);
        Assert.AreEqual("ibm", timeSeries.Symbol);
    }

    [TestMethod]
    public void FromDocument_WithMissingMetaData_ReturnsNull()
    {
        // Arrange
        var jsonWithoutMeta = @"
{
    ""Time Series (Daily)"": {
        ""2025-12-08"": {
            ""1. open"": ""309.6200"",
            ""2. high"": ""315.3454"",
            ""3. low"": ""307.9500"",
            ""4. close"": ""309.1800"",
            ""5. volume"": ""3615794""
        }
    }
}";
        var document = JsonDocument.Parse(jsonWithoutMeta);

        // Act
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNull(timeSeries);
    }

    [TestMethod]
    public void FromDocument_WithMissingTimeSeriesProperty_ReturnsNull()
    {
        // Arrange
        var jsonWithoutTimeSeries = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    }
}";
        var document = JsonDocument.Parse(jsonWithoutTimeSeries);

        // Act
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNull(timeSeries);
    }

    [TestMethod]
    public void FromDocument_WithEmptyTimeSeriesData_ReturnsEmptyData()
    {
        // Arrange
        var jsonWithEmptyData = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {}
}";
        var document = JsonDocument.Parse(jsonWithEmptyData);

        // Act
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNotNull(timeSeries);
        Assert.HasCount(0, timeSeries.Data);
    }

    [TestMethod]
    public void FromDocument_WithInvalidDateKeys_SkipsInvalidEntries()
    {
        // Arrange
        var jsonWithInvalidDates = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {
        ""2025-12-08"": {
            ""1. open"": ""309.6200"",
            ""2. high"": ""315.3454"",
            ""3. low"": ""307.9500"",
            ""4. close"": ""309.1800"",
            ""5. volume"": ""3615794""
        },
        ""invalid-date"": {
            ""1. open"": ""100.00"",
            ""2. high"": ""101.00"",
            ""3. low"": ""99.00"",
            ""4. close"": ""100.50"",
            ""5. volume"": ""1000""
        },
        ""2025-12-05"": {
            ""1. open"": ""308.5900"",
            ""2. high"": ""311.8300"",
            ""3. low"": ""307.1800"",
            ""4. close"": ""307.9400"",
            ""5. volume"": ""2344667""
        }
    }
}";
        var document = JsonDocument.Parse(jsonWithInvalidDates);

        // Act
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNotNull(timeSeries);
        Assert.HasCount(2, timeSeries.Data); // Only valid dates are included
        Assert.IsTrue(timeSeries.Data.ContainsKey(new DateOnly(2025, 12, 8)));
        Assert.IsTrue(timeSeries.Data.ContainsKey(new DateOnly(2025, 12, 5)));
        Assert.IsFalse(timeSeries.Data.Any(x => x.Key == default));
    }

    [TestMethod]
    public void FromDocument_WithMissingSymbolInMetadata_ThrowsArgumentException()
    {
        // Arrange
        var jsonWithoutSymbol = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {}
}";
        var document = JsonDocument.Parse(jsonWithoutSymbol);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);
        });
    }

    [TestMethod]
    public void FromDocument_WithMissingLastRefreshed_ThrowsFormatException()
    {
        // Arrange
        var jsonWithoutLastRefreshed = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {}
}";
        var document = JsonDocument.Parse(jsonWithoutLastRefreshed);

        // Act & Assert
        Assert.ThrowsExactly<FormatException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);
        });
    }

    [TestMethod]
    public void FromDocument_WithAllIntervalsSupported_ParsesCorrectly()
    {
        // Test Daily
        var dailyResult = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonDaily), "IBM", TimeSeries.TimeSeriesInterval.Daily);
        Assert.IsNotNull(dailyResult);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Daily, dailyResult.Interval);

        // Test Weekly
        var weeklyResult = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonWeekly), "IBM", TimeSeries.TimeSeriesInterval.Weekly);
        Assert.IsNotNull(weeklyResult);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Weekly, weeklyResult.Interval);

        // Test Monthly
        var monthlyResult = TimeSeriesParser.FromDocument(JsonDocument.Parse(jsonMonthly), "IBM", TimeSeries.TimeSeriesInterval.Monthly);
        Assert.IsNotNull(monthlyResult);
        Assert.AreEqual(TimeSeries.TimeSeriesInterval.Monthly, monthlyResult.Interval);
    }

    [TestMethod]
    public void FromDocument_WithPartialDataFields_UsesDefaults()
    {
        // Arrange
        var jsonWithPartialFields = @"
{
    ""Meta Data"": {
        ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
        ""2. Symbol"": ""IBM"",
        ""3. Last Refreshed"": ""2025-12-08"",
        ""4. Output Size"": ""Compact"",
        ""5. Time Zone"": ""US/Eastern""
    },
    ""Time Series (Daily)"": {
        ""2025-12-08"": {
            ""1. open"": ""309.6200"",
            ""3. low"": ""307.9500""
        }
    }
}";
        var document = JsonDocument.Parse(jsonWithPartialFields);

        // Act
        var timeSeries = TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.Daily);

        // Assert
        Assert.IsNotNull(timeSeries);
        Assert.HasCount(1, timeSeries.Data);
        var entry = timeSeries.Data.First();
        Assert.AreEqual(309.62m, entry.Value.Open);
        Assert.AreEqual(0m, entry.Value.High);
        Assert.AreEqual(307.95m, entry.Value.Low);
        Assert.AreEqual(0m, entry.Value.Close);
        Assert.AreEqual(0L, entry.Value.Volume);
    }

    [TestMethod]
    public void FromDocument_UnsupportedInterval_OneMin_ThrowsNotSupportedException()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act & Assert
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.OneMin);
        });
    }

    [TestMethod]
    public void FromDocument_UnsupportedInterval_FiveMin_ThrowsNotSupportedException()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act & Assert
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.FiveMin);
        });
    }

    [TestMethod]
    public void FromDocument_UnsupportedInterval_FifteenMin_ThrowsNotSupportedException()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act & Assert
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.FifteenMin);
        });
    }

    [TestMethod]
    public void FromDocument_UnsupportedInterval_ThirtyMin_ThrowsNotSupportedException()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act & Assert
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.ThirtyMin);
        });
    }

    [TestMethod]
    public void FromDocument_UnsupportedInterval_SixtyMin_ThrowsNotSupportedException()
    {
        // Arrange
        var document = JsonDocument.Parse(jsonDaily);

        // Act & Assert
        Assert.ThrowsExactly<NotSupportedException>(() =>
        {
            TimeSeriesParser.FromDocument(document, "IBM", TimeSeries.TimeSeriesInterval.SixtyMin);
        });
    }
}
