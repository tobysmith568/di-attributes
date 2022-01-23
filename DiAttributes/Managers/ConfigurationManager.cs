using DiAttributes.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiAttributes.Managers;

internal class ConfigurationManager : IManager
{
    private const string NullConfigurationException = "A class was decorated with the Configuration attribute " +
        $"but {nameof(RegisterExtensions.RegisterDiAttributes)} was called without passing in an {nameof(IConfiguration)}.";

    private readonly IServiceCollection services;
    private readonly IConfiguration? configuration;
    private MethodInfo? cachedConfigurationMethod;

    public ConfigurationManager(IServiceCollection services, IConfiguration? configuration)
    {
        this.services = services;
        this.configuration = configuration;
    }

    public void Register(Type @class, CustomAttributeData customAttributeData)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration), NullConfigurationException);

        if (cachedConfigurationMethod == null)
            cachedConfigurationMethod = GetAddConfigurationExtensionMethod();

        if (customAttributeData.ConstructorArguments.Count != 1)
            return;

        var key = (string)customAttributeData.ConstructorArguments[0].Value;

        try
        {
            var configurationMethod = cachedConfigurationMethod.MakeGenericMethod(@class);
            configurationMethod.Invoke(services, new object[] { services, configuration.GetSection(key) });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unabled to configure the class {@class.FullName} with the key '{key}'", ex);
        }
    }

    private static MethodInfo GetAddConfigurationExtensionMethod()
    {
        MethodInfo extensionMethod;
        try
        {
            extensionMethod = Assembly.Load("Microsoft.Extensions.Options.ConfigurationExtensions")
                .GetAllExtensionMethods()
                .WithMethodName("Configure")
                .WithParameters(typeof(IServiceCollection), typeof(IConfiguration))
                .SingleOrDefault();
        }
        catch (InvalidOperationException ex)
        {
            const string ErrorMessage = "Found more than one IServiceCollection.Configure extension method";
            throw new InvalidOperationException(ErrorMessage, ex);
        }

        if (extensionMethod == null)
        {
            const string ErrorMessage = "Unable to find the IServiceCollection.Configure extension method";
            throw new InvalidOperationException(ErrorMessage);
        }

        return extensionMethod;
    }
}
