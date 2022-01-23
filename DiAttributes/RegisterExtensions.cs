using DiAttributes.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiAttributes;

public static class RegisterExtensions
{
    /// <summary>
    /// Registers all classes that are decorated with the Scoped, Transient, Singleton, and HttpClient attributes.
    /// 
    /// To use the Configuration attribute you need to use the overload which accepts an IConfiguration.
    /// </summary>
    public static void RegisterDiAttributes(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services), $"The given {nameof(IServiceCollection)} was null.");

        RegisterClasses(services, null);
    }

    /// <summary>
    /// Registers all classes that are decorated with the Scoped, Transient, Singleton, HttpClient, and Configuration attributes.
    /// 
    /// If you're not using the Configuration attribute then you can use the overload which doesn't need an IConfiguration.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void RegisterDiAttributes(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services), $"The given {nameof(IServiceCollection)} was null.");

        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration), $"The given {nameof(IConfiguration)} was null.");

        RegisterClasses(services, configuration);
    }

    private static void RegisterClasses(IServiceCollection services, IConfiguration? configuration)
    {
        var managerFactory = new ManagerFactory(services, configuration);

        IEnumerable<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .Where(t => t.IsClass && t.Name[0] != '<');

        foreach (var @class in classes)
        {
            RegisterClass(@class, managerFactory);
        }
    }

    private static void RegisterClass(Type @class, ManagerFactory managerFactory)
    {
        var diAttributes = @class.CustomAttributes
            .Where(a => typeof(IDiAttribute).IsAssignableFrom(a.AttributeType))
            .GroupBy(a => a.AttributeType, (key, results) => new { AttributeType = key, Attributes = results })
            .FirstOrDefault();

        if (diAttributes == null)
            return;

        var manager = managerFactory.GetManager(diAttributes.AttributeType);

        foreach (var attribute in diAttributes.Attributes)
        {
            manager.Register(@class, attribute);
        }
    }
}