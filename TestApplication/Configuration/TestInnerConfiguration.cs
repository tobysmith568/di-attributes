using DiAttributes;

namespace TestApplication.Configuration;

[Configuration("AnotherOuter:Inner")]
public class TestInnerConfiguration
{
    public int MyNumber { get; set; }
    public string? MyString { get; set; }
}
