using DiAttributes;

namespace TestApplication.Configuration;

[Configuration("Outer")]
public class TestOuterConfiguration
{
    public int MyNumber { get; set; }
    public string? MyString { get; set; }
}
