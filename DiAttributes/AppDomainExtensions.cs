using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DiAttributes
{
    internal static class AppDomainExtensions
    {
        internal static IEnumerable<MethodInfo> GetExtensionMethodsInAssembly(this AppDomain appDomain, string assemblyName)
        {
            const BindingFlags StaticMethodBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var allAssemblies = appDomain.GetAssemblies();

            var configurationAssemblies = allAssemblies.Where(a =>
            {
                var assemblyNameWithComma = assemblyName + ',';
                return a.FullName.StartsWith(assemblyNameWithComma);
            });

            var allTypes = configurationAssemblies.SelectMany(a => a.GetTypes());

            var sealedNonGenericTypes = allTypes
                .Where(t => t.IsSealed && !t.IsGenericType && !t.IsNested);

            var extensionMethods = sealedNonGenericTypes
                .SelectMany(t => t.GetMethods(StaticMethodBindingFlags))
                .Where(method => method.IsDefined(typeof(ExtensionAttribute), false));

            return extensionMethods;
        }
    }
}
