using System;

namespace DiAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        public SingletonAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public SingletonAttribute()
        {
        }

        public Type ServiceType { get; }
    }
}
