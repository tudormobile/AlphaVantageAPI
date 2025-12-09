using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tudormobile.AlphaVantage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageClientTests
{
    [TestMethod]
    public void Constructor_WithValidApiKey_SetsApiKeyField()
    {
        // Arrange
        var expected = "test-api-key-123";

        // Act
        var client = new AlphaVantageClient(expected);

        // Assert
        var field = typeof(AlphaVantageClient).GetField("_apiKey", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.IsNotNull(field);
        var actual = field.GetValue(client) as string;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void Constructor_WithNullApiKey_ThrowsArgumentException()
    {
        // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void Constructor_WithEmptyApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient(string.Empty));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void Constructor_WithWhitespaceApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient("   "));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod, ExcludeFromCodeCoverage]
    public void Constructor_WithWhitespaceTabsApiKey_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => new AlphaVantageClient("\t\t"));
        Assert.AreEqual("apiKey", ex.ParamName);
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithValidParameters()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Global Quote", out _));
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithInvalidParameters()
    {
        // Arrange
        var apiKey = "invalid-key";
        var client = new AlphaVantageClient(apiKey);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "invalid-symbol";    // Use something other than IBM to avoid demo success
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Global Quote", out var quote));
        Assert.IsTrue(quote.GetRawText().Trim().Equals("{}"));
    }

    [TestMethod]
    public async Task GetJsonDocumentAsync_WithInvalidDemoKey()
    {
        // Arrange
        var apiKey = "demo";
        var client = new AlphaVantageClient(apiKey);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "invalid-symbol";    // Use something other than IBM to avoid demo success
        // Act
        var jsonDocument = await client.GetJsonDocumentAsync(function, symbol);
        // Assert
        Assert.IsNotNull(jsonDocument);
        Assert.IsFalse(jsonDocument.RootElement.TryGetProperty("Global Quote", out _));
        Assert.IsTrue(jsonDocument.RootElement.TryGetProperty("Information", out _));
    }

    [TestMethod]
    public async Task GetJsonStringAsync_WithValidParameters()
    {
        // Arrange
        var apiKey = "demo"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey);
        var function = AlphaVantageFunction.GLOBAL_QUOTE;
        var symbol = "IBM";
        // Act
        var jsonString = await client.GetJsonStringAsync(function, symbol);
        // Assert
        Assert.IsNotNull(jsonString);
        Assert.Contains("\"Global Quote\"", jsonString);
        Assert.Contains("\"IBM\"", jsonString);
    }

    [TestMethod]
    public async Task GetJsonStringAsync_Withsss()
    {
        // Arrange
        var apiKey = "test-key"; // Alpha Vantage provides a demo API key for testing
        var client = new AlphaVantageClient(apiKey);
        var function = AlphaVantageFunction.SYMBOL_SEARCH;
        var symbol = "Microsoft Corp";
        // Act
        var jsonString = await client.GetJsonStringAsync(function, symbol);
        // Assert
        Assert.IsNotNull(jsonString);
        Assert.Contains("\"bestMatches\"", jsonString);
        Assert.Contains("\"MSFT\"", jsonString);
    }
}

