using Tudormobile.AlphaVantage.Extensions;

namespace AlphaVantageAPI.Tests;

[TestClass]
public class AlphaVantageOptionsTests
{
    [TestMethod]
    public void Constructor_InitializesWithEmptyApiKey()
    {
        // Act
        var options = new AlphaVantageOptions();

        // Assert
        Assert.IsNotNull(options);
        Assert.AreEqual(string.Empty, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeSet()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        const string expectedApiKey = "TEST_API_KEY_123";

        // Act
        options.ApiKey = expectedApiKey;

        // Assert
        Assert.AreEqual(expectedApiKey, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeSetToNull()
    {
        // Arrange
        var options = new AlphaVantageOptions
        {
            ApiKey = "INITIAL_KEY"
        };

        // Act
        options.ApiKey = null!;

        // Assert
        Assert.IsNull(options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeSetToEmptyString()
    {
        // Arrange
        var options = new AlphaVantageOptions
        {
            ApiKey = "INITIAL_KEY"
        };

        // Act
        options.ApiKey = string.Empty;

        // Assert
        Assert.AreEqual(string.Empty, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeSetWithWhitespace()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        const string whitespaceKey = "   ";

        // Act
        options.ApiKey = whitespaceKey;

        // Assert
        Assert.AreEqual(whitespaceKey, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeInitializedWithObjectInitializer()
    {
        // Arrange
        const string expectedApiKey = "OBJECT_INIT_KEY";

        // Act
        var options = new AlphaVantageOptions
        {
            ApiKey = expectedApiKey
        };

        // Assert
        Assert.AreEqual(expectedApiKey, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_CanBeUpdatedMultipleTimes()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        const string firstKey = "FIRST_KEY";
        const string secondKey = "SECOND_KEY";
        const string thirdKey = "THIRD_KEY";

        // Act
        options.ApiKey = firstKey;
        Assert.AreEqual(firstKey, options.ApiKey);

        options.ApiKey = secondKey;
        Assert.AreEqual(secondKey, options.ApiKey);

        options.ApiKey = thirdKey;

        // Assert
        Assert.AreEqual(thirdKey, options.ApiKey);
    }

    [TestMethod]
    public void ApiKey_SupportsLongStrings()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        var longApiKey = new string('A', 1000);

        // Act
        options.ApiKey = longApiKey;

        // Assert
        Assert.AreEqual(longApiKey, options.ApiKey);
        Assert.AreEqual(1000, options.ApiKey.Length);
    }

    [TestMethod]
    public void ApiKey_SupportsSpecialCharacters()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        const string specialCharKey = "API-KEY_123.test@example!#$%";

        // Act
        options.ApiKey = specialCharKey;

        // Assert
        Assert.AreEqual(specialCharKey, options.ApiKey);
    }

    [TestMethod]
    public void MultipleInstances_AreIndependent()
    {
        // Arrange
        const string firstKey = "FIRST_INSTANCE_KEY";
        const string secondKey = "SECOND_INSTANCE_KEY";

        // Act
        var options1 = new AlphaVantageOptions { ApiKey = firstKey };
        var options2 = new AlphaVantageOptions { ApiKey = secondKey };

        // Assert
        Assert.AreEqual(firstKey, options1.ApiKey);
        Assert.AreEqual(secondKey, options2.ApiKey);
        Assert.AreNotEqual(options1.ApiKey, options2.ApiKey);
    }

    [TestMethod]
    public void ApiKey_DoesNotTrimWhitespace()
    {
        // Arrange
        var options = new AlphaVantageOptions();
        const string keyWithSpaces = "  KEY_WITH_SPACES  ";

        // Act
        options.ApiKey = keyWithSpaces;

        // Assert
        Assert.AreEqual(keyWithSpaces, options.ApiKey);
        Assert.AreEqual(19, options.ApiKey.Length);
    }
}
