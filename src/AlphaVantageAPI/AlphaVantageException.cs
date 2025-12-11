namespace Tudormobile.AlphaVantage;

/// <summary>
/// Represents errors that occur during Alpha Vantage API operations.
/// </summary>
/// <remarks>
/// This exception is thrown when API requests fail due to network issues, invalid responses, rate limiting,
/// authentication failures, or other Alpha Vantage service-related errors. Check the inner exception for
/// underlying HTTP or network errors.
/// </remarks>
public class AlphaVantageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaVantageException"/> class.
    /// </summary>
    public AlphaVantageException() { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaVantageException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AlphaVantageException(string message)
        : base(message) { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AlphaVantageException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
    public AlphaVantageException(string message, Exception inner)
        : base(message, inner) { }
}
