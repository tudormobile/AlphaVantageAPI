namespace Tudormobile.AlphaVantage;

internal class AlphaVantageClientBuilder : IAlphaVantageClientBuilder
{
    private string _apiKey = string.Empty;

    public IAlphaVantageClient Build()
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException("API key must be set before building the client. Use WithApiKey() to set the API key.");
        }
        return new AlphaVantageClient(_apiKey);
    }

    public IAlphaVantageClientBuilder WithApiKey(string apiKey)
    {
        _apiKey = apiKey;
        return this;
    }
}
