using System.Reflection;

namespace DiAttributes.Extensions;

internal static class MethodInfoEnumerableExtensions
{
    public static IEnumerable<MethodInfo> WithMethodName(this IEnumerable<MethodInfo> query, string methodName) =>
        query.Where(method => method.Name == methodName);

    public static IEnumerable<MethodInfo> WithNumberOfGenericArguments(this IEnumerable<MethodInfo> query, int numberOfGenericArguments) =>
        query.Where(method => method.GetGenericArguments().Length == numberOfGenericArguments);

    public static IEnumerable<MethodInfo> WithParameters(this IEnumerable<MethodInfo> query, params Type[] parameterTypes) =>
        query = query.Where(method =>
        {
            var parameters = method.GetParameters();

            if (parameters.Length != parameterTypes.Length)
                return false;

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                    return false;
            }

            return true;
        });
}
