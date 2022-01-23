using DiAttributes.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes.Managers;

internal class HttpClientManager : IManager
{
    private readonly IServiceCollection services;

    private MethodInfo? cachedAddHttpClientMethodWithServiceType;
    private MethodInfo? cachedAddHttpClientMethodWithoutServiceType;

    internal HttpClientManager(IServiceCollection services)
    {
        this.services = services;
    }

    public void Register(Type @class, CustomAttributeData customAttributeData)
    {
        var hasServiceType = customAttributeData.ConstructorArguments.Count == 1;

        var addHttpClientMethod = hasServiceType
            ? GetRegisterMethodWithServiceType(@class, customAttributeData)
            : GetRegisterMethodWithoutServiceType(@class);

        try
        {
            addHttpClientMethod.Invoke(services, new object[] { services });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unabled to register the class '{@class.FullName}' as an HttpClient", ex);
        }
    }

    private MethodInfo GetRegisterMethodWithServiceType(Type @class, CustomAttributeData customAttributeData)
    {
        if (cachedAddHttpClientMethodWithServiceType == null)
            cachedAddHttpClientMethodWithServiceType = GetRegisterMethodForNumberOfGenerics(2);

        var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;

        var genericArguments = new Type[] { serviceType, @class };

        var addHttpClientMethod = cachedAddHttpClientMethodWithServiceType.MakeGenericMethod(genericArguments);
        return addHttpClientMethod;
    }

    private MethodInfo GetRegisterMethodWithoutServiceType(Type @class)
    {
        if (cachedAddHttpClientMethodWithoutServiceType == null)
            cachedAddHttpClientMethodWithoutServiceType = GetRegisterMethodForNumberOfGenerics(1);

        var genericArguments = new Type[] { @class };

        var addHttpClientMethod = cachedAddHttpClientMethodWithoutServiceType.MakeGenericMethod(genericArguments);
        return addHttpClientMethod;
    }

    private static MethodInfo GetRegisterMethodForNumberOfGenerics(int numberOfGenericArgs)
    {
        MethodInfo extensionMethod;
        try
        {
            extensionMethod = Assembly.Load("Microsoft.Extensions.Http")
                .GetAllExtensionMethods()
                .WithMethodName("AddHttpClient")
                .WithNumberOfGenericArguments(numberOfGenericArgs)
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
