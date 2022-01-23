using DiAttributes.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes;

/// <summary>
/// Apply this attribute to a class to register it an injectable configuration option.  
/// 
/// To use this attribute, be sure to pass an <c>IConfiguration</c> into the
/// <c>IServiceCollection.RegisterDiAttributes()</c> call in your startup file.
/// 
/// Use the <c>key</c> argument to specify the path in your configuration to bind against
/// 
/// e.g. to bind a class against a configuration like this:
/// 
/// <example> 
/// <code>
///     {
///         "Outer": {
///             "Inner": {
///                 "MySetting": "My Value"
///             }
///         }
///     }
/// </code>
/// 
/// Register it like this:
/// 
/// <code>
///     [Configuration("Outer:Inner")]  
///     public class MyConfig
///     {
///         public string MySetting { get; set; }
///     }
/// </code>
/// </example>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ConfigurationAttribute : Attribute, IDiAttribute
{
    /// <param name="key">The path in your configuration to bind the class against</param>
    public ConfigurationAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; }
}
