using DiAttributes;

namespace TestApplication.Services;

public interface ITestHttpClient
{
    HttpClient HttpClient { get; }

    string GetSubjectName();
}

[HttpClient]
[HttpClient(typeof(ITestHttpClient))]
public class TestHttpClientService : ITestHttpClient
{
    public HttpClient HttpClient { get; }

    public TestHttpClientService(HttpClient httpClient) =>
        HttpClient = httpClient;

    public string GetSubjectName() => nameof(TestHttpClientService);
}
