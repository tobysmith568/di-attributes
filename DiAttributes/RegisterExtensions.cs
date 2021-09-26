using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DiAttributes
{
    public static class RegisterExtensions
    {
        public static void RegisterDiAttributes(this IServiceCollection services)
        {
            if (services == null)
            {
                return;
            }

            IEnumerable<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.Name[0] != '<');

            foreach (var @class in classes)
            {
                foreach (var customAttribute in @class.CustomAttributes)
                {
                    TryRegisterScopedAttribute(services, @class, customAttribute);
                    TryRegisterSingletonAttribute(services, @class, customAttribute);
                    TryRegisterTransientAttribute(services, @class, customAttribute);
                }
            }
        }

        private static void TryRegisterScopedAttribute(IServiceCollection services, Type injectable, CustomAttributeData customAttributeData)
        {
            if (customAttributeData.AttributeType != typeof(ScopedAttribute))
            {
                return;
            }

            if (customAttributeData.ConstructorArguments.Count > 0)
            {
                var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
                services.AddScoped(serviceType, injectable);
            }
            else
            {
                services.AddScoped(injectable);
            }
        }

        private static void TryRegisterSingletonAttribute(IServiceCollection services, Type injectable, CustomAttributeData customAttributeData)
        {
            if (customAttributeData.AttributeType != typeof(SingletonAttribute))
            {
                return;
            }

            if (customAttributeData.ConstructorArguments.Count > 0)
            {
                var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
                services.AddSingleton(serviceType, injectable);
            }
            else
            {
                services.AddSingleton(injectable);
            }
        }

        private static void TryRegisterTransientAttribute(IServiceCollection services, Type injectable, CustomAttributeData customAttributeData)
        {
            if (customAttributeData.AttributeType != typeof(TransientAttribute))
            {
                return;
            }

            if (customAttributeData.ConstructorArguments.Count > 0)
            {
                var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
                services.AddTransient(serviceType, injectable);
            }
            else
            {
                services.AddTransient(injectable);
            }
        }
    }
}
