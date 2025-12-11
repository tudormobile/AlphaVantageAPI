namespace Tudormobile.AlphaVantage.Entities;

/// <summary>
/// Represents a generic response wrapper for Alpha Vantage API calls.
/// Provides a standardized way to handle both successful responses and error conditions.
/// </summary>
/// <typeparam name="T">The type of the data returned by the API call. Must be a reference type.</typeparam>
public class AlphaVantageResponse<T> where T : class
{
    /// <summary>
    /// Gets or sets the error message if the API call failed.
    /// This will be null or empty for successful API calls.
    /// </summary>
    /// <value>A string containing the error message, or null if the call was successful.</value>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Gets or sets the result data from the API call.
    /// This will be null if the API call failed or returned no data.
    /// </summary>
    /// <value>An instance of type T containing the API response data, or null if unsuccessful.</value>
    public T? Result { get; init; }

    /// <summary>
    /// Gets a value indicating whether the API call was successful.
    /// A call is considered successful if there is no error message and the result is not null.
    /// </summary>
    /// <value>true if the API call was successful; otherwise, false.</value>
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage) && Result != null;
}
