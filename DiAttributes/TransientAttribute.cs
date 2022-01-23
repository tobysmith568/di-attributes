#pragma warning disable IDE0060 // Remove unused parameter

namespace DiAttributes;

/// <summary>
/// Apply this attribute to a class to register it as a transient dependency in the IoC container.
/// 
/// Use the parameter to register the class against a service type
/// 
/// e.g.
/// 
/// <code>
///     [Transient(typeof(IMyService))]  
///     public class MyService : IMyService  
///     { }
/// </code>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientAttribute : Attribute, IDiAttribute
{
    /// <param name="serviceType">The service type to register the class against; usually an interface</param>
    public TransientAttribute(Type serviceType)
    {
    }

    public TransientAttribute()
    {
    }
}