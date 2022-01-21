using System;

namespace DiAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpClientAttribute : Attribute
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
}
