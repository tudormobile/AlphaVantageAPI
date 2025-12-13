using System.Text.Json;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class EntityParserTests
{
    private enum TestEnum
    {
        Value1,
        Value2,
        TestValue
    }

    [TestMethod]
    public void GetDecimalValue_WithValidProperty_ReturnsDecimal()
    {
        // Arrange
        var jsonString = "{\"price\":\"123.45\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDecimalValue(jsonDocument.RootElement, "price");

        // Assert
        Assert.AreEqual(123.45m, result);
    }

    [TestMethod]
    public void GetDecimalValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDecimalValue(jsonDocument.RootElement, "price", 99.99m);

        // Assert
        Assert.AreEqual(99.99m, result);
    }

    [TestMethod]
    public void GetDoubleValue_WithValidProperty_ReturnsDouble()
    {
        // Arrange
        var jsonString = "{\"value\":\"456.78\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDoubleValue(jsonDocument.RootElement, "value");

        // Assert
        Assert.AreEqual(456.78, result);
    }

    [TestMethod]
    public void GetDoubleValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDoubleValue(jsonDocument.RootElement, "value", 100.0);

        // Assert
        Assert.AreEqual(100.0, result);
    }

    [TestMethod]
    public void GetDateOnlyValue_WithValidProperty_ReturnsDateOnly()
    {
        // Arrange
        var jsonString = "{\"date\":\"2025-12-12\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDateOnlyValue(jsonDocument.RootElement, "date");

        // Assert
        Assert.AreEqual(new DateOnly(2025, 12, 12), result);
    }

    [TestMethod]
    public void GetDateOnlyValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        var defaultDate = new DateOnly(2020, 1, 1);
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDateOnlyValue(jsonDocument.RootElement, "date", defaultDate);

        // Assert
        Assert.AreEqual(defaultDate, result);
    }

    [TestMethod]
    public void GetTimeOnlyValue_WithValidProperty_ReturnsTimeOnly()
    {
        // Arrange
        var jsonString = "{\"time\":\"14:30:00\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetTimeOnlyValue(jsonDocument.RootElement, "time");

        // Assert
        Assert.AreEqual(new TimeOnly(14, 30, 0), result);
    }

    [TestMethod]
    public void GetTimeOnlyValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        var defaultTime = new TimeOnly(12, 0, 0);
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetTimeOnlyValue(jsonDocument.RootElement, "time", defaultTime);

        // Assert
        Assert.AreEqual(defaultTime, result);
    }

    [TestMethod]
    public void GetLongValue_WithValidProperty_ReturnsLong()
    {
        // Arrange
        var jsonString = "{\"count\":\"9876543210\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetLongValue(jsonDocument.RootElement, "count");

        // Assert
        Assert.AreEqual(9876543210L, result);
    }

    [TestMethod]
    public void GetLongValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetLongValue(jsonDocument.RootElement, "count", 1000L);

        // Assert
        Assert.AreEqual(1000L, result);
    }

    [TestMethod]
    public void GetStringValue_WithValidProperty_ReturnsString()
    {
        // Arrange
        var jsonString = "{\"name\":\"Test\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetStringValue(jsonDocument.RootElement, "name");

        // Assert
        Assert.AreEqual("Test", result);
    }

    [TestMethod]
    public void GetStringValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetStringValue(jsonDocument.RootElement, "name", "Default");

        // Assert
        Assert.AreEqual("Default", result);
    }

    [TestMethod]
    public void GetEnumValue_WithValidProperty_ReturnsEnum()
    {
        // Arrange
        var jsonString = "{\"status\":\"Value1\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetEnumValue(jsonDocument.RootElement, "status", TestEnum.Value2);

        // Assert
        Assert.AreEqual(TestEnum.Value1, result);
    }

    [TestMethod]
    public void GetEnumValue_WithMissingProperty_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetEnumValue(jsonDocument.RootElement, "status", TestEnum.Value2);

        // Assert
        Assert.AreEqual(TestEnum.Value2, result);
    }

    [TestMethod]
    public void GetEnumValue_WithCaseInsensitiveMatch_ReturnsEnum()
    {
        // Arrange
        var jsonString = "{\"status\":\"testvalue\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetEnumValue(jsonDocument.RootElement, "status", TestEnum.Value1);

        // Assert
        Assert.AreEqual(TestEnum.TestValue, result);
    }

    [TestMethod]
    public void GetDecimalValue_WithInvalidValue_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{\"price\":\"invalid\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetDecimalValue(jsonDocument.RootElement, "price", 50m);

        // Assert
        Assert.AreEqual(50m, result);
    }

    [TestMethod]
    public void GetStringValue_WithNullValue_ReturnsDefault()
    {
        // Arrange
        var jsonString = "{\"name\":null}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var result = EntityParser.GetStringValue(jsonDocument.RootElement, "name", "Fallback");

        // Assert
        Assert.AreEqual("Fallback", result);
    }
}
