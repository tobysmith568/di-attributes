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
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute : Attribute
{
    /// <param name="key">The path in your configuration to bind the class against</param>
    public ConfigurationAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; }
}

internal static class ConfigurationTypeExtensions
{
    private static MethodInfo? cachedConfigurationMethod;

    internal static void RegisterAsConfiguration(this Type @class, CustomAttributeData customAttributeData, IServiceCollection services, IConfiguration configuration)
    {
        if (cachedConfigurationMethod == null)
            cachedConfigurationMethod = GetAddConfigurationExtensionMethod();

        if (customAttributeData.ConstructorArguments.Count != 1)
            return;

        var key = (string)customAttributeData.ConstructorArguments[0].Value;

        try
        {
            var configurationMethod = cachedConfigurationMethod.MakeGenericMethod(@class);
            configurationMethod.Invoke(services, new object[] { services, configuration.GetSection(key) });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unabled to configure the class {@class.FullName} with the key '{key}'", ex);
        }
    }

    private static MethodInfo GetAddConfigurationExtensionMethod()
    {
        MethodInfo extensionMethod;
        try
        {
            extensionMethod = Assembly.Load("Microsoft.Extensions.Options.ConfigurationExtensions")
                .GetAllExtensionMethods()
                .WithMethodName("Configure")
                .WithParameters(typeof(IServiceCollection), typeof(IConfiguration))
                .SingleOrDefault();
        }
        catch (InvalidOperationException ex)
        {
            const string ErrorMessage = "Found more than one IServiceCollection.Configure extension method";
            throw new InvalidOperationException(ErrorMessage, ex);
        }

        if (extensionMethod == null)
        {
            const string ErrorMessage = "Unable to find the IServiceCollection.Configure extension method";
            throw new InvalidOperationException(ErrorMessage);
        }

        return extensionMethod;
    }
}
