using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using TestApplication.Configuration;

namespace TestApplication.UnitTests;

public class ConfigurationTests
{
    HttpClient httpClient;

    private readonly ConfigurationResponse expectedConfigurationResponseResult = new()
    {
        Outer = new TestOuterConfiguration
        {
            MyNumber = 5,
            MyString = "Test String 5"
        },
        Inner = new TestInnerConfiguration
        {
            MyNumber = 6,
            MyString = "Test String 6"
        },
        Dictionary = new TestDictionaryConfiguration
        {
            { "Key1", "Value 1" },
            { "Key2", "Value 2" },
            { "Key3", "Value 3" }
        }
    };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var application = new WebApplicationFactory<Program>();
        httpClient = application.CreateClient();
    }

    [Test]
    public async Task TheConfigurationIsCorrectlyPopulated()
    {
        var response = await httpClient.GetAsync("/Configuration");
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var stringResult = await response.Content.ReadAsStringAsync();
        stringResult.Should().NotBeNull();

        var actualResult = JsonSerializer.Deserialize<ConfigurationResponse>(stringResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        actualResult.Should().BeEquivalentTo(expectedConfigurationResponseResult);
    }
}
