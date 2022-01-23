using DiAttributes;

namespace TestApplication.Services;

public interface ITestSingletonService
{
    string GetSubjectName();
}

[Singleton]
[Singleton(typeof(ITestSingletonService))]
public class TestSingletonService : ITestSingletonService
{
    public string GetSubjectName() => nameof(TestSingletonService);
}
