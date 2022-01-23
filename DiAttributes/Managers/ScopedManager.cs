using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes.Managers;

internal class ScopedManager : IManager
{
    private readonly IServiceCollection services;

    internal ScopedManager(IServiceCollection services)
    {
        this.services = services;
    }

    public void Register(Type @class, CustomAttributeData customAttributeData)
    {
        if (customAttributeData.ConstructorArguments.Count == 0)
        {
            services.AddScoped(@class);
            return;
        }

        var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
        services.AddScoped(serviceType, @class);
    }
}
