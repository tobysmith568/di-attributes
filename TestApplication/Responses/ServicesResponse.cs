namespace TestApplication.Responses;

public class ServicesResponse
{
    public string? Scoped { get; set; }
    public string? Singleton { get; set; }
    public string? Transient { get; set; }
    public string? HttpClient { get; set; }
}
