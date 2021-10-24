using System;

namespace DiAttributes
{
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
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientAttribute : Attribute
    {
        /// <param name="serviceType">The service type to register the class against; usually an interface</param>
        public TransientAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public TransientAttribute()
        {
        }

        public Type? ServiceType { get; }
    }
}
