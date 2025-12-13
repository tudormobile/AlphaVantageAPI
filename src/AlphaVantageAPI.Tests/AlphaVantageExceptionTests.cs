using Tudormobile.AlphaVantage;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageExceptionTests
{
    [TestMethod]
    public void DefaultConstructor_CreatesException()
    {
        // Act
        var exception = new AlphaVantageException();

        // Assert
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType<Exception>(exception);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Constructor_WithMessage_SetsMessage()
    {
        // Arrange
        const string expectedMessage = "API request failed";

        // Act
        var exception = new AlphaVantageException(expectedMessage);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(expectedMessage, exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Constructor_WithNullMessage_AcceptsNull()
    {
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var exception = new AlphaVantageException(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Assert
        Assert.IsNotNull(exception);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Constructor_WithEmptyMessage_AcceptsEmptyString()
    {
        // Act
        var exception = new AlphaVantageException(string.Empty);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(string.Empty, exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Constructor_WithMessageAndInnerException_SetsBothProperties()
    {
        // Arrange
        const string expectedMessage = "API request failed";
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new AlphaVantageException(expectedMessage, innerException);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(expectedMessage, exception.Message);
        Assert.AreSame(innerException, exception.InnerException);
    }

    [TestMethod]
    public void Constructor_WithMessageAndNullInnerException_AcceptsNull()
    {
        // Arrange
        const string expectedMessage = "API request failed";

        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var exception = new AlphaVantageException(expectedMessage, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual(expectedMessage, exception.Message);
        Assert.IsNull(exception.InnerException);
    }

    [TestMethod]
    public void Exception_CanBeThrown()
    {
        // Arrange
        const string expectedMessage = "Rate limit exceeded";

        // Act & Assert
        var exception = Assert.ThrowsExactly<AlphaVantageException>(() =>
        {
            throw new AlphaVantageException(expectedMessage);
        });

        Assert.AreEqual(expectedMessage, exception.Message);
    }

    [TestMethod]
    public void Exception_CanBeCaughtAsException()
    {
        // Arrange
        const string expectedMessage = "Authentication failed";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        Exception caughtException = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
        try
        {
            throw new AlphaVantageException(expectedMessage);
        }
        catch (Exception ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.IsNotNull(caughtException);
        Assert.IsInstanceOfType<AlphaVantageException>(caughtException);
        Assert.AreEqual(expectedMessage, caughtException.Message);
    }

    [TestMethod]
    public void Exception_PreservesInnerExceptionStackTrace()
    {
        // Arrange
        var innerException = new HttpRequestException("Network error");
        const string message = "Failed to fetch data";

        // Act
        var exception = new AlphaVantageException(message, innerException);

        // Assert
        Assert.AreSame(innerException, exception.InnerException);
        Assert.IsInstanceOfType<HttpRequestException>(exception.InnerException);
    }
}
