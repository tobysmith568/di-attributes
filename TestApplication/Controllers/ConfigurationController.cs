using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestApplication.Configuration;
using TestApplication.Responses;

namespace TestApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IOptions<TestOuterConfiguration> outerConfiguration;
    private readonly IOptions<TestInnerConfiguration> innerConfiguration;
    private readonly IOptions<TestDictionaryConfiguration> dictionaryConfiguration;

    public ConfigurationController(
        IOptions<TestOuterConfiguration> outerConfiguration,
        IOptions<TestInnerConfiguration> innerConfiguration,
        IOptions<TestDictionaryConfiguration> dictionaryConfiguration)
    {
        this.outerConfiguration = outerConfiguration;
        this.innerConfiguration = innerConfiguration;
        this.dictionaryConfiguration = dictionaryConfiguration;
    }

    [HttpGet]
    public ConfigurationResponse GetDependencyNames()
    {
        return new ConfigurationResponse
        {
            Outer = outerConfiguration.Value,
            Inner = innerConfiguration.Value,
            Dictionary = dictionaryConfiguration.Value
        };
    }
}
