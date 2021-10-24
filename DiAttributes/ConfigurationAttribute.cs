using System;

namespace DiAttributes
{
    /// <summary>
    /// Apply this attribute to a class to register it an injectable configuration option.  
    /// 
    /// To use this attribute, be sure to pass an <c>IConfiguration</c> into the
    /// <c>IServiceCollection.RegisterDiAttributes()</c> call in your startup file.
    /// 
    /// Use the <c>key</c> argument to specify the path in your configuration to bind against
    /// 
    /// e.g.
    /// 
    /// <example> 
    /// <code>
    ///     {
    ///         "Outer": {
    ///             "Inner": {
    ///                 "MySetting": "My Value"
    ///             }
    ///         }
    ///     }
    /// </code>
    /// 
    /// <code>
    ///     [Configuration("Outer:Inner")]  
    ///     public class MyConfig
    ///     {
    ///         public string MySetting { get; set; }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {
        /// <param name="key">The path in your configuration to bind the class against</param>
        public ConfigurationAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
