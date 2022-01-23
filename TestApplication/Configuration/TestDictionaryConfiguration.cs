using DiAttributes;

namespace TestApplication.Configuration;

[Configuration("MyDictionary")]
public class TestDictionaryConfiguration : Dictionary<string, string>
{
}
