using DiAttributes;

namespace TestApplication.Services;

public interface ITestTransientService
{
    string GetSubjectName();
}

[Transient]
[Transient(typeof(ITestTransientService))]
public class TestTransientService : ITestTransientService
{
    public string GetSubjectName() => nameof(TestTransientService);
}
