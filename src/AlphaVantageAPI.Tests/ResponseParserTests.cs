using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Tudormobile.AlphaVantage.Entities;
using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class ResponseParserTests
{
    // Test entity for use in generic tests
    [ExcludeFromCodeCoverage]
    private class TestEntity : IEntity
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    [TestMethod]
    public void CreateResponse_WithValidResult_ReturnsSuccessResponse()
    {
        // Arrange
        var testEntity = new TestEntity { Name = "Test", Value = 42 };
        var jsonString = "{\"data\":\"test\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Result);
        Assert.AreEqual(testEntity, response.Result);
        Assert.IsNull(response.ErrorMessage);
        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullResult_ReturnsErrorResponse()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"data\":\"test\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNull(response.Result);
        Assert.IsNotNull(response.ErrorMessage);
        Assert.AreEqual("Unknown error occurred.", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullResultAndCustomErrorMessage_UsesCustomMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"data\":\"test\"}";
        var customErrorMessage = "Custom error message";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument, customErrorMessage);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNull(response.Result);
        Assert.AreEqual(customErrorMessage, response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullResultAndInformationProperty_ReturnsInformationMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Information\":\"API rate limit reached\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNull(response.Result);
        Assert.AreEqual("API rate limit reached", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullResultAndErrorMessageProperty_ReturnsErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Error Message\":\"Invalid API key\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNull(response.Result);
        Assert.AreEqual("Invalid API key", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithBothInformationAndErrorMessage_PrioritizesInformation()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Information\":\"Rate limit message\",\"Error Message\":\"Error message\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Rate limit message", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithEmptyInformationProperty_FallsBackToErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Information\":\"\",\"Error Message\":\"Fallback error\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Fallback error", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithEmptyErrorMessageProperty_FallsBackToDefault()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Error Message\":\"\"}";
        var defaultMessage = "Default error";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument, defaultMessage);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(defaultMessage, response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullInformationValue_FallsBackToErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Information\":null,\"Error Message\":\"Error occurred\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Error occurred", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithNullErrorMessageValue_FallsBackToDefault()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Error Message\":null}";
        var defaultMessage = "Default fallback";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument, defaultMessage);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(defaultMessage, response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithEmptyJsonObject_UsesDefaultErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Unknown error occurred.", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithComplexJsonAndValidResult_IgnoresErrorProperties()
    {
        // Arrange
        var testEntity = new TestEntity { Name = "Success", Value = 100 };
        var jsonString = "{\"Information\":\"Should be ignored\",\"Error Message\":\"Also ignored\",\"data\":\"value\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Result);
        Assert.AreEqual(testEntity, response.Result);
        Assert.IsNull(response.ErrorMessage);
        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithWhitespaceInformationValue_FallsBackToErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Information\":\"   \",\"Error Message\":\"Actual error\"}";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Actual error", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithWhitespaceErrorMessageValue_FallsBackToDefault()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "{\"Error Message\":\"   \"}";
        var defaultMessage = "No valid error found";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument, defaultMessage);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual(defaultMessage, response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public void CreateResponse_WithJsonArray_UsesDefaultErrorMessage()
    {
        // Arrange
        TestEntity? testEntity = null;
        var jsonString = "[{\"item\":1},{\"item\":2}]";
        using var jsonDocument = JsonDocument.Parse(jsonString);

        // Act
        var response = ResponseParser.CreateResponse(testEntity, jsonDocument);

        // Assert
        Assert.IsNotNull(response);
        Assert.AreEqual("Unknown error occurred.", response.ErrorMessage);
        Assert.IsFalse(response.IsSuccess);
    }
}
