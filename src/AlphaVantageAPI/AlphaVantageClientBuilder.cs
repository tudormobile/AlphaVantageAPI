using Microsoft.Extensions.Logging;

namespace Tudormobile.AlphaVantage;

internal class AlphaVantageClientBuilder : IAlphaVantageClientBuilder
{
    private string _apiKey = string.Empty;
    private HttpClient? _httpClient;
    private ILogger? _logger;

    public string ApiKey
    {
        get => _apiKey;
        set => _apiKey = value;
    }

    public IAlphaVantageClient Build()
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException("API key must be set before building the client. Use WithApiKey() to set the API key.");
        }
        if (_httpClient == null)
        {
            throw new InvalidOperationException("An HttpClient instance must be provided. Use WithHttpClient() to indicate what client instance to use.");
        }
        return new AlphaVantageClient(_apiKey, _httpClient, _logger);
    }

    public IAlphaVantageClientBuilder WithApiKey(string apiKey)
    {
        _apiKey = apiKey;
        return this;
    }

    public IAlphaVantageClientBuilder WithHttpClient(HttpClient client)
    {
        _httpClient = client;
        return this;
    }

    public IAlphaVantageClientBuilder AddLogging(ILogger logger)
    {
        _logger = logger;
        return this;
    }

}
