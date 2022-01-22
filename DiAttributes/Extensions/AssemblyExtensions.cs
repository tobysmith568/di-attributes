using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DiAttributes.Extensions
{
    internal static class AssemblyExtensions
    {
        internal static IEnumerable<MethodInfo> GetAllExtensionMethods(this Assembly assembly)
        {
            const BindingFlags StaticMethodBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var allTypes = assembly.GetTypes();

            var sealedNonGenericTypes = allTypes
                .Where(t => t.IsSealed && !t.IsGenericType && !t.IsNested);

            var extensionMethods = sealedNonGenericTypes
                .SelectMany(t => t.GetMethods(StaticMethodBindingFlags))
                .Where(method => method.IsDefined(typeof(ExtensionAttribute), false));

            return extensionMethods;
        }
    }
}
