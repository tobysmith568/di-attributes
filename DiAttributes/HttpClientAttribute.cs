using DiAttributes.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes;

/// <summary>
/// Apply this attribute to a class to register it as an HTTP Client dependency in the IoC container.
/// This type of dependency is Transient.
/// 
/// Use the parameter to register the class against a service type
/// 
/// e.g.
/// 
/// <code>
///     [HttpClient(typeof(IMyService))]  
///     public class MyService : IMyService  
///     { }
/// </code>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HttpClientAttribute : Attribute, IDiAttribute
{
    /// <param name="serviceType">The service type to register the class against; usually an interface</param>
    public HttpClientAttribute(Type serviceType)
    {
        ServiceType = serviceType;
    }

    public HttpClientAttribute()
    {
    }

    public Type? ServiceType { get; }
}