using System;

namespace DiAttributes
{
    /// <summary>
    /// Apply this attribute to a class to register it as a scoped dependency in the IoC container.
    /// 
    /// Use the parameter to register the class against a service type
    /// 
    /// e.g.
    /// 
    /// <code>
    ///     [Scoped(typeof(IMyService))]  
    ///     public class MyService : IMyService  
    ///     { }
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ScopedAttribute : Attribute
    {
        /// <param name="serviceType">The service type to register the class against; usually an interface</param>
        public ScopedAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public ScopedAttribute()
        {
        }

        public Type? ServiceType { get; }
    }
}
