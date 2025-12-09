namespace Tudormobile.AlphaVantage;

/// <summary>
/// Defines a mechanism for constructing an instance of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of object to be constructed by the builder.</typeparam>
public interface IBuilder<T>
{
    /// <summary>
    /// Creates and returns an instance of type <typeparamref name="T"/> based on the current configuration.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/> representing the constructed object.</returns>
    T Build();
}