using System;

namespace DiAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        public ConfigurationAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
