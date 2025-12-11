namespace Tudormobile.AlphaVantage.Extensions;

/// <summary>
/// Gets or sets the API key used to authenticate requests to the Alpha Vantage service.
/// </summary>
/// <remarks>The API key is required for accessing Alpha Vantage endpoints. Obtain an API key from the Alpha
/// Vantage website and assign it to this property before making requests.</remarks>
public class AlphaVantageOptions
{
    /// <summary>
    /// Gets or sets the API key used to authenticate requests to external services.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}