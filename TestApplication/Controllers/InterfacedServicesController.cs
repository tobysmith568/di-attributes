using Microsoft.AspNetCore.Mvc;
using TestApplication.Responses;
using TestApplication.Services;

namespace TestApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class InterfacedServicesController : ControllerBase
{
    private readonly ITestSingletonService testSingletonService;
    private readonly ITestScopedService testScopedService;
    private readonly ITestTransientService testTransientService;
    private readonly ITestHttpClient testHttpClient;

    public InterfacedServicesController(
        ITestSingletonService testSingletonService,
        ITestScopedService testScopedService,
        ITestTransientService testTransientService,
        ITestHttpClient testHttpClient)
    {
        this.testSingletonService = testSingletonService;
        this.testScopedService = testScopedService;
        this.testTransientService = testTransientService;
        this.testHttpClient = testHttpClient;
    }

    [HttpGet]
    public ServicesResponse GetDependencyNames()
    {
        return new ServicesResponse
        {
            Scoped = testScopedService?.GetSubjectName() ?? $"Failed to inject {nameof(ITestScopedService)}",
            Singleton = testSingletonService?.GetSubjectName() ?? $"Failed to inject {nameof(ITestSingletonService)}",
            Transient = testTransientService?.GetSubjectName() ?? $"Failed to inject {nameof(ITestTransientService)}",
            HttpClient = testHttpClient?.GetSubjectName() ?? $"Failed to inject {nameof(ITestHttpClient)}",
        };
    }
}
