using System;
using System.Collections.Generic;
using System.Text;

namespace Tudormobile.AlphaVantage;

/// <summary>
/// Defines a builder for configuring and creating instances of <see cref="IAlphaVantageClient"/>.
/// </summary>
/// <remarks>Use this interface to fluently configure API credentials and other options before constructing an
/// <see cref="IAlphaVantageClient"/>. The builder pattern enables step-by-step configuration and validation prior to
/// client creation.</remarks>
public interface IAlphaVantageClientBuilder : IBuilder<IAlphaVantageClient>
{
    /// <summary>
    /// Sets the API key to be used for authenticating requests to the Alpha Vantage service.
    /// </summary>
    /// <param name="apiKey">The API key provided by Alpha Vantage. Cannot be null or empty.</param>
    /// <returns>The current builder instance with the specified API key configured.</returns>
    IAlphaVantageClientBuilder WithApiKey(string apiKey);
}
