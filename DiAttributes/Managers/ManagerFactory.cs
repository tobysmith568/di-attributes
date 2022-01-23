using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiAttributes.Managers;

internal class ManagerFactory
{
    private readonly Dictionary<Type, IManager> managers;

    public ManagerFactory(IServiceCollection services, IConfiguration? configuration)
    {
        managers = new Dictionary<Type, IManager>
        {
            { typeof(ScopedAttribute), new ScopedManager(services) },
            { typeof(SingletonAttribute), new SingletonManager(services) },
            { typeof(TransientAttribute), new TransientManager(services) },
            { typeof(HttpClientAttribute), new HttpClientManager(services) },
            { typeof(ConfigurationAttribute), new ConfigurationManager(services, configuration) }
        };
    }

    internal IManager GetManager(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (!typeof(IDiAttribute).IsAssignableFrom(type))
            throw new ArgumentException($"The given type needs to extend {nameof(IDiAttribute)}", nameof(type));

        var foundManager = managers.TryGetValue(type, out IManager manager);

        if (foundManager)
            return manager;

        throw new InvalidOperationException($"Unknown DiAttribute: {type.FullName}");
    }
}
