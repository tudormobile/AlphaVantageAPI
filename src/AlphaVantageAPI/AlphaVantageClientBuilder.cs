using System;
using System.Collections.Generic;
using System.Text;

namespace Tudormobile.AlphaVantage;

internal class AlphaVantageClientBuilder : IAlphaVantageClientBuilder
{
    private string _apiKey = String.Empty;
    public IAlphaVantageClient Build() => new AlphaVantageClient(_apiKey);
    public IAlphaVantageClientBuilder WithApiKey(string apiKey)
    {
        _apiKey = apiKey;
        return this;
    }
}
