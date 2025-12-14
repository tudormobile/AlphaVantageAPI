using System.Text.Json;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class DividendsParserTests
{
    private readonly string validJson = @"
{
    ""symbol"" : ""IBM"",
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""0.2300""
        },
        {
            ""declaration_date"": ""2023-05-09"",
            ""ex_dividend_date"": ""2023-05-10"",
            ""record_date"": ""2023-05-11"",
            ""payment_date"": ""2023-06-10"",
            ""amount"": ""0.2300""
        }
    ]
}";

    [TestMethod]
    public void FromDocument_WithValidData_ParsesCorrectly()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("IBM", result.Symbol);
        Assert.HasCount(2, result.Data);

        var firstDividend = result.Data[0];
        Assert.AreEqual(new DateOnly(2023, 8, 10), firstDividend.DeclarationDate);
        Assert.AreEqual(new DateOnly(2023, 8, 11), firstDividend.ExDividendDate);
        Assert.AreEqual(new DateOnly(2023, 8, 14), firstDividend.RecordDate);
        Assert.AreEqual(new DateOnly(2023, 9, 1), firstDividend.PaymentDate);
        Assert.AreEqual(0.2300m, firstDividend.Amount);

        var secondDividend = result.Data[1];
        Assert.AreEqual(new DateOnly(2023, 5, 9), secondDividend.DeclarationDate);
        Assert.AreEqual(new DateOnly(2023, 5, 10), secondDividend.ExDividendDate);
        Assert.AreEqual(new DateOnly(2023, 5, 11), secondDividend.RecordDate);
        Assert.AreEqual(new DateOnly(2023, 6, 10), secondDividend.PaymentDate);
        Assert.AreEqual(0.2300m, secondDividend.Amount);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDocument_ReturnsNull()
    {
        // Arrange
        var document = JsonDocument.Parse("{}");

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FromDocument_WithEmptyDataArray_ReturnsEmptyList()
    {
        // Arrange
        var json = @"{ ""data"": [] }";
        var document = JsonDocument.Parse(json);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("IBM", result.Symbol);
        Assert.IsEmpty(result.Data);
    }

    [TestMethod]
    public void FromDocument_WithNullValues_UsesDefaults()
    {
        // Arrange
        var jsonWithNulls = @"
{
    ""data"": [
        {
            ""declaration_date"": null,
            ""ex_dividend_date"": null,
            ""record_date"": null,
            ""payment_date"": null,
            ""amount"": null
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithNulls);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        var dividend = result.Data[0];
        Assert.AreEqual(default, dividend.DeclarationDate);
        Assert.AreEqual(default, dividend.ExDividendDate);
        Assert.AreEqual(default, dividend.RecordDate);
        Assert.AreEqual(default, dividend.PaymentDate);
        Assert.AreEqual(0m, dividend.Amount);
    }

    [TestMethod]
    public void FromDocument_WithMissingFields_UsesDefaults()
    {
        // Arrange
        var jsonWithMissingFields = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""amount"": ""0.2300""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithMissingFields);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        var dividend = result.Data[0];
        Assert.AreEqual(new DateOnly(2023, 8, 10), dividend.DeclarationDate);
        Assert.AreEqual(default, dividend.ExDividendDate);
        Assert.AreEqual(default, dividend.RecordDate);
        Assert.AreEqual(default, dividend.PaymentDate);
        Assert.AreEqual(0.2300m, dividend.Amount);
    }

    [TestMethod]
    public void FromDocument_WithInvalidDateFormat_UsesDefaultDate()
    {
        // Arrange
        var jsonWithInvalidDate = @"
{
    ""data"": [
        {
            ""declaration_date"": ""invalid-date"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""0.2300""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithInvalidDate);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        var dividend = result.Data[0];
        Assert.AreEqual(default, dividend.DeclarationDate);
        Assert.AreEqual(new DateOnly(2023, 8, 11), dividend.ExDividendDate);
    }

    [TestMethod]
    public void FromDocument_WithInvalidAmount_UsesDefaultAmount()
    {
        // Arrange
        var jsonWithInvalidAmount = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""invalid-amount""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonWithInvalidAmount);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        Assert.AreEqual(0m, result.Data[0].Amount);
    }

    [TestMethod]
    public void FromDocument_WithDataAsNonArray_ReturnsNull()
    {
        // Arrange
        var jsonWithNonArray = @"{ ""data"": ""not-an-array"" }";
        var document = JsonDocument.Parse(jsonWithNonArray);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FromDocument_WithSingleDividend_ParsesCorrectly()
    {
        // Arrange
        var jsonSingle = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""0.5500""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonSingle);

        // Act
        var result = DividendsParser.FromDocument(document, "AAPL");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("AAPL", result.Symbol);
        Assert.HasCount(1, result.Data);
        Assert.AreEqual(0.5500m, result.Data[0].Amount);
    }

    [TestMethod]
    public void FromDocument_WithMultipleDividends_ParsesAllCorrectly()
    {
        // Arrange
        var jsonMultiple = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""0.23""
        },
        {
            ""declaration_date"": ""2023-05-09"",
            ""ex_dividend_date"": ""2023-05-10"",
            ""record_date"": ""2023-05-11"",
            ""payment_date"": ""2023-06-10"",
            ""amount"": ""0.23""
        },
        {
            ""declaration_date"": ""2023-02-08"",
            ""ex_dividend_date"": ""2023-02-09"",
            ""record_date"": ""2023-02-10"",
            ""payment_date"": ""2023-03-10"",
            ""amount"": ""0.22""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonMultiple);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(3, result.Data);
        Assert.AreEqual(0.23m, result.Data[0].Amount);
        Assert.AreEqual(0.23m, result.Data[1].Amount);
        Assert.AreEqual(0.22m, result.Data[2].Amount);
    }

    [TestMethod]
    public void FromDocument_WithZeroAmount_ParsesCorrectly()
    {
        // Arrange
        var jsonZeroAmount = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""0.0000""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonZeroAmount);

        // Act
        var result = DividendsParser.FromDocument(document, "IBM");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        Assert.AreEqual(0m, result.Data[0].Amount);
    }

    [TestMethod]
    public void FromDocument_WithLargeAmount_ParsesCorrectly()
    {
        // Arrange
        var jsonLargeAmount = @"
{
    ""data"": [
        {
            ""declaration_date"": ""2023-08-10"",
            ""ex_dividend_date"": ""2023-08-11"",
            ""record_date"": ""2023-08-14"",
            ""payment_date"": ""2023-09-01"",
            ""amount"": ""999.9999""
        }
    ]
}";
        var document = JsonDocument.Parse(jsonLargeAmount);

        // Act
        var result = DividendsParser.FromDocument(document, "BRK.A");

        // Assert
        Assert.IsNotNull(result);
        Assert.HasCount(1, result.Data);
        Assert.AreEqual(999.9999m, result.Data[0].Amount);
    }

    [TestMethod]
    public void FromDocument_WithDifferentSymbols_PreservesSymbol()
    {
        // Arrange
        var document = JsonDocument.Parse(validJson);

        // Act
        var result1 = DividendsParser.FromDocument(document, "MSFT");
        var result2 = DividendsParser.FromDocument(document, "AAPL");

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsNotNull(result2);
        Assert.AreEqual("MSFT", result1.Symbol);
        Assert.AreEqual("AAPL", result2.Symbol);
    }
}
