using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using TestApplication.Services;

namespace TestApplication.UnitTests;

public class ServicesTests
{
    HttpClient httpClient;

    private readonly ServicesResponse expectedServicesResponseResult = new()
    {
        Scoped = nameof(TestScopedService),
        Singleton = nameof(TestSingletonService),
        Transient = nameof(TestTransientService),
        HttpClient = nameof(TestHttpClientService),
    };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var application = new WebApplicationFactory<Program>();
        httpClient = application.CreateClient();
    }

    [Test]
    public async Task TheDirectServicesAreCorrectlyPopulated()
    {
        var response = await httpClient.GetAsync("/DirectServices");
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var stringResult = await response.Content.ReadAsStringAsync();
        stringResult.Should().NotBeNull();

        var actualResult = JsonSerializer.Deserialize<ServicesResponse>(stringResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        actualResult.Should().BeEquivalentTo(expectedServicesResponseResult);
    }

    [Test]
    public async Task TheInterfacesServicesAreCorrectlyPopulated()
    {
        var response = await httpClient.GetAsync("/InterfacedServices");
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var stringResult = await response.Content.ReadAsStringAsync();
        stringResult.Should().NotBeNull();

        var actualResult = JsonSerializer.Deserialize<ServicesResponse>(stringResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        actualResult.Should().BeEquivalentTo(expectedServicesResponseResult);
    }
}
