using Microsoft.AspNetCore.Mvc;
using TestApplication.Responses;
using TestApplication.Services;

namespace TestApplication.Controllers;

[Route("[controller]")]
[ApiController]
public class DirectServicesController : ControllerBase
{
    private readonly TestSingletonService testSingletonService;
    private readonly TestScopedService testScopedService;
    private readonly TestTransientService testTransientService;
    private readonly TestHttpClientService testHttpClient;

    public DirectServicesController(
        TestSingletonService testSingletonService,
        TestScopedService testScopedService,
        TestTransientService testTransientService,
        TestHttpClientService testHttpClient)
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
            Scoped = testScopedService?.GetSubjectName() ?? $"Failed to inject {nameof(TestScopedService)}",
            Singleton = testSingletonService?.GetSubjectName() ?? $"Failed to inject {nameof(TestSingletonService)}",
            Transient = testTransientService?.GetSubjectName() ?? $"Failed to inject {nameof(TestTransientService)}",
            HttpClient = testHttpClient?.GetSubjectName() ?? $"Failed to inject {nameof(TestHttpClientService)}",
        };
    }
}
