using TestApplication.Configuration;

namespace TestApplication.Responses;

public class ConfigurationResponse
{
    public TestOuterConfiguration? Outer { get; set; }
    public TestInnerConfiguration? Inner { get; set; }
    public TestDictionaryConfiguration? Dictionary { get; set; }
}
