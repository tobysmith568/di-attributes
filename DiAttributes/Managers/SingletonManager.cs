using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes.Managers;

internal class SingletonManager : IManager
{
    private readonly IServiceCollection services;

    internal SingletonManager(IServiceCollection services)
    {
        this.services = services;
    }

    public void Register(Type @class, CustomAttributeData customAttributeData)
    {
        if (customAttributeData.ConstructorArguments.Count == 0)
        {
            services.AddSingleton(@class);
            return;
        }

        var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
        services.AddSingleton(serviceType, @class);
    }
}
