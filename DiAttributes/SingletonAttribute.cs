using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes;

/// <summary>
/// Apply this attribute to a class to register it as a singleton dependency in the IoC container.
/// 
/// Use the parameter to register the class against a service type
/// 
/// e.g.
/// 
/// <code>
///     [Singleton(typeof(IMyService))]  
///     public class MyService : IMyService  
///     { }
/// </code>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonAttribute : Attribute, IDiAttribute
{
    /// <param name="serviceType">The service type to register the class against; usually an interface</param>
    public SingletonAttribute(Type serviceType)
    {
        ServiceType = serviceType;
    }

    public SingletonAttribute()
    {
    }

    public Type? ServiceType { get; }
}
