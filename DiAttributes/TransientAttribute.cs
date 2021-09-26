using System;

namespace DiAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TransientAttribute : Attribute
    {
        public TransientAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public TransientAttribute()
        {
        }

        public Type ServiceType { get; }
    }
}
