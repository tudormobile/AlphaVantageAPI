using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class TreasuryYieldParserTests
{
    private readonly string validJson = @"
{
    ""name"": ""10-Year Treasury Constant Maturity Rate"",
    ""interval"": ""monthly"",
    ""unit"": ""percent"",
    ""data"": [
        {
            ""date"": ""2025-11-01"",
            ""value"": ""4.09""
        },
        {
            ""date"": ""2025-10-01"",
            ""value"": ""4.06""
        }
    ]
}";

    [TestMethod]
    public void FromDocument_WithValidData_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Monthly, TreasuryYieldMaturity._10Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("10-Year Treasury Constant Maturity Rate", result.Name);
        Assert.AreEqual(TreasuryYieldInterval.Monthly, result.Interval);
        Assert.AreEqual(TreasuryYieldMaturity._10Year, result.Maturity);
        Assert.AreEqual("percent", result.Unit);
        Assert.HasCount(2, result.Data);

        var firstData = result.Data[0];
        Assert.AreEqual(new DateOnly(2025, 11, 1), firstData.Date);
        Assert.AreEqual(4.09m, firstData.Value);

        var secondData = result.Data[1];
        Assert.AreEqual(new DateOnly(2025, 10, 1), secondData.Date);
        Assert.AreEqual(4.06m, secondData.Value);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDocument_ReturnsNull()
    {
        // Arrange
        var document = JsonDocument.Parse("{}");

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._10Year);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDataArray_ReturnsEmptyList()
    {
        // Arrange
        var json = @"
{
    ""name"": ""10-Year Treasury Constant Maturity Rate"",
    ""interval"": ""monthly"",
    ""unit"": ""percent"",
    ""data"": []
}";
        var document = JsonDocument.Parse(json);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Monthly, TreasuryYieldMaturity._10Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("10-Year Treasury Constant Maturity Rate", result.Name);
        Assert.IsEmpty(result.Data);
    }

    [TestMethod]
    public void FromDocument_WithDailyInterval_ParsesCorrectly()
    {
        // Arrange
        var json = @"
{
    ""name"": ""10-Year Treasury Constant Maturity Rate"",
    ""interval"": ""daily"",
    ""unit"": ""percent"",
    ""data"": [
        {
            ""date"": ""2025-11-01"",
            ""value"": ""4.09""
        }
    ]
}";
        var document = JsonDocument.Parse(json);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._10Year);
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldInterval.Daily, result.Interval);
    }

    [TestMethod]
    public void FromDocument_WithWeeklyInterval_ParsesCorrectly()
    {
        // Arrange
        var json = @"
{
    ""name"": ""10-Year Treasury Constant Maturity Rate"",
    ""interval"": ""weekly"",
    ""unit"": ""percent"",
    ""data"": [
        {
            ""date"": ""2025-11-01"",
            ""value"": ""4.09""
        }
    ]
}";
        var document = JsonDocument.Parse(json);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Weekly, TreasuryYieldMaturity._10Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldInterval.Weekly, result.Interval);
    }

    [TestMethod]
    public void FromDocument_With3MonthMaturity_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Monthly, TreasuryYieldMaturity._3Month);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._3Month, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_With2YearMaturity_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._2Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._2Year, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_With5YearMaturity_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._5Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._5Year, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_With7YearMaturity_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._7Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._7Year, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_With30YearMaturity_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._30Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._30Year, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_WithUnknownMaturity_DefaultsTo10Year()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, (TreasuryYieldMaturity)999);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(TreasuryYieldMaturity._10Year, result.Maturity);
    }

    [TestMethod]
    public void FromDocument_WithNullValues_UsesDefaults()
    {
        // Arrange
        var jsonWithNulls = @"
{
    ""name"": ""10-Year Treasury Constant Maturity Rate"",
    ""interval"": ""monthly"",
    ""unit"": ""percent"",
    ""data"": [
        {
            ""date"": null,
            ""value"": null
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithNulls);

        // Act
        var result = TreasuryYieldParser.FromDocument(document, TreasuryYieldInterval.Daily, TreasuryYieldMaturity._10Year);

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        Assert.AreEqual(default, result.Data[0].Date);
        Assert.AreEqual(0m, result.Data[0].Value);
    }
}
