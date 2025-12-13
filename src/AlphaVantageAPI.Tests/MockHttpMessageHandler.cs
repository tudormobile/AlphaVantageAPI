namespace AlphaVantageAPI.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public Exception? AlwaysThrows { get; set; } = null;
    public HttpResponseMessage? AlwaysResponds { get; set; } = null;
    public string JsonResponse { get; } = @"{
                ""Global Quote"": {
                    ""01. symbol"": ""IBM"",
                    ""02. open"": ""125.0000"",
                    ""03. high"": ""127.0000"",
                    ""04. low"": ""124.5000"",
                    ""05. price"": ""126.0000"",
                    ""06. volume"": ""3000000"",
                    ""07. latest trading day"": ""2023-10-01"",
                    ""08. previous close"": ""125.5000"",
                    ""09. change"": ""0.5000"",
                    ""10. change percent"": ""0.3984%""
                }
            }";
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            if (AlwaysThrows != null)
            {
                throw AlwaysThrows;
            }
            if (AlwaysResponds != null)
            {
                return AlwaysResponds;
            }
            if (request!.RequestUri!.Query.Contains("ABCDEFG"))
            {
                var json = @"{
            ""Information"": ""The **demo** API key is for demo purposes only. Please claim your free API key at (https://www.alphavantage.co/support/#api-key) to explore our full API offerings. It takes fewer than 20 seconds.""
}";
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else if (request.RequestUri.Query.Contains("function=TIME_SERIES_DAILY"))
            {
                var json = @"{
                ""Meta Data"": {
                    ""1. Information"": ""Daily Prices (open, high, low, close) and Volumes"",
                    ""2. Symbol"": ""IBM"",
                    ""3. Last Refreshed"": ""2023-10-01"",
                    ""4. Output Size"": ""Compact"",
                    ""5. Time Zone"": ""US/Eastern""
                },
                ""Time Series (Daily)"": {
                    ""2023-10-01"": {
                        ""1. open"": ""125.0000"",
                        ""2. high"": ""127.0000"",
                        ""3. low"": ""124.5000"",
                        ""4. close"": ""126.0000"",
                        ""5. volume"": ""3000000""
                    }
                }
            }";
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            else if (request.RequestUri.Query.Contains("function=TIME_SERIES_MONTHLY"))
            {
                var json = @"{
                ""Meta Data"": {
                    ""1. Information"": ""Monthly Prices (open, high, low, close) and Volumes"",
                    ""2. Symbol"": ""IBM"",
                    ""3. Last Refreshed"": ""2023-10-01"",
                    ""4. Time Zone"": ""US/Eastern""
                },
                ""Monthly Time Series"": {
                    ""2023-10-01"": {
                        ""1. open"": ""120.0000"",
                        ""2. high"": ""130.0000"",
                        ""3. low"": ""119.0000"",
                        ""4. close"": ""126.0000"",
                        ""5. volume"": ""90000000""
                    }
                }
            }";
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonResponse, System.Text.Encoding.UTF8, "application/json")
            };
        });
    }
}
