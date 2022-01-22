using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiAttributes
{
    public static class RegisterExtensions
    {
        private const string NullConfigurationException = "A class was decorated with the Configuration attribute " +
            "but RegisterDiAttributes was called without passing in an IConfiguration.";

        /// <summary>
        /// Registers all classes that are decorated with the Scoped, Transient, Singleton, and HttpClient attributes.
        /// 
        /// To use the Configuration attribute you need to use the overload which accepts an IConfiguration.
        /// </summary>
        public static void RegisterDiAttributes(this IServiceCollection services)
        {
            Register(services, null);
        }

        /// <summary>
        /// Registers all classes that are decorated with the Scoped, Transient, Singleton, HttpClient, and Configuration attributes.
        /// 
        /// If you're not using the Configuration attribute then you can use the overload which doesn't need an IConfiguration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void RegisterDiAttributes(this IServiceCollection services, IConfiguration configuration)
        {
            Register(services, configuration);
        }

        private static void Register(this IServiceCollection services, IConfiguration? configuration)
        {
            if (services == null)
                return;

            IEnumerable<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.Name[0] != '<');

            foreach (var @class in classes)
            {
                TryRegisterClass(@class, services, configuration);
            }
        }

        private static void TryRegisterClass(Type @class, IServiceCollection services, IConfiguration? configuration)
        {
            foreach (var customAttribute in @class.CustomAttributes)
            {
                if (customAttribute.AttributeType == typeof(ScopedAttribute))
                {
                    @class.RegisterAsScoped(customAttribute, services);
                    return;
                }

                if (customAttribute.AttributeType == typeof(SingletonAttribute))
                {
                    @class.RegisterAsSingleton(customAttribute, services);
                    return;
                }

                if (customAttribute.AttributeType == typeof(TransientAttribute))
                {
                    @class.RegisterAsTransient(customAttribute, services);
                    return;
                }

                if (customAttribute.AttributeType == typeof(HttpClientAttribute))
                {
                    @class.RegisterAsHttpClient(customAttribute, services);
                    return;
                }

                if (customAttribute.AttributeType == typeof(ConfigurationAttribute))
                {
                    if (configuration == null)
                        throw new ArgumentNullException(nameof(configuration), NullConfigurationException);

                    @class.RegisterAsConfiguration(customAttribute, services, configuration);
                    return;
                }
            }
        }
    }
}
