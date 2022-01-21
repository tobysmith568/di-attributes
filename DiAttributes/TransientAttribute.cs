using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

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

    internal static class TransientTypeExtensions
    {
        internal static void RegisterAsTransient(this Type @class, CustomAttributeData customAttributeData, IServiceCollection services)
        {
            if (customAttributeData.ConstructorArguments.Count == 0)
            {
                services.AddTransient(@class);
                return;
            }

            var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
            services.AddTransient(serviceType, @class);
        }
    }
}
