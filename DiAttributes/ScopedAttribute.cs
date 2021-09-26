using System;

namespace DiAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScopedAttribute : Attribute
    {
        public ScopedAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public ScopedAttribute()
        {
        }

        public Type ServiceType { get; }
    }
}
