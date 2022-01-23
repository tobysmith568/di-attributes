using DiAttributes;

namespace TestApplication.Services;

public interface ITestScopedService
{
    string GetSubjectName();
}

[Scoped]
[Scoped(typeof(ITestScopedService))]
public class TestScopedService : ITestScopedService
{
    public string GetSubjectName() => nameof(TestScopedService);
}
