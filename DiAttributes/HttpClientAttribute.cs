using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

    internal static class HttpClientTypeExtensions
    {
        private static MethodInfo? cachedAddHttpClientMethod;

        internal static void RegisterAsHttpClient(this Type @class, CustomAttributeData customAttributeData, IServiceCollection services)
        {
            if (cachedAddHttpClientMethod == null)
            {
                cachedAddHttpClientMethod = GetAddHttpClientExtensionMethod();
            }

            var genericArguments = new List<Type>(2);

            if (customAttributeData.ConstructorArguments.Count == 1)
            {
                var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
                genericArguments.Add(serviceType);
            }

            genericArguments.Add(@class);

            var addHttpClientMethod = cachedAddHttpClientMethod.MakeGenericMethod(genericArguments.ToArray());

            try
            {
                addHttpClientMethod.Invoke(services, new object[] { services });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unabled to register the class {@class.FullName} as an HttpClient", ex);
            }
        }

        private static MethodInfo GetAddHttpClientExtensionMethod()
        {
            MethodInfo extensionMethod;
            try
            {
                extensionMethod = AppDomain.CurrentDomain
                    .GetExtensionMethodsInAssembly("Microsoft.Extensions.Http")
                    .WithMethodName("AddHttpClient")
                    .WithNumberOfGenericArguments(2)
                    .WithParameters(typeof(IServiceCollection))
                    .SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                const string ErrorMessage = "Found more than one IServiceCollection.AddHttpClient extension method";
                throw new InvalidOperationException(ErrorMessage, ex);
            }

            if (extensionMethod == null)
            {
                const string ErrorMessage = "Unable to find the IServiceCollection.AddHttpClient extension method";
                throw new InvalidOperationException(ErrorMessage);
            }

            return extensionMethod;
        }
    }
}
