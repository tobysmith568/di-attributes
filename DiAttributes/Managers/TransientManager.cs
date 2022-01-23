using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes.Managers
{
    internal class TransientManager : IManager
    {
        private readonly IServiceCollection services;

        internal TransientManager(IServiceCollection services)
        {
            this.services = services;
        }

        public void Register(Type @class, CustomAttributeData customAttributeData)
        {
            if (customAttributeData.ConstructorArguments.Count == 0)
            {
                services.AddTransient(@class);
                return;
            }

            var serviceType = (Type)customAttributeData.ConstructorArguments[0].Value;
            services.AddTransient(serviceType, @class);
        }
    }
}
