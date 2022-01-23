# DiAttributes

Super-small and super-simple library for registering classes with the ASP.NET Core `IServiceCollection` using attributes.

<a href="https://www.nuget.org/packages/DiAttributes">
  <img alt="npm" src="https://img.shields.io/nuget/v/DiAttributes?logo=nuget">
</a>
<a href="https://codecov.io/gh/tobysmith568/di-attributes">
  <img src="https://codecov.io/gh/tobysmith568/di-attributes/branch/main/graph/badge.svg"/>
</a>

GitHub: https://github.com/tobysmith568/di-attributes  
NuGet: https://www.nuget.org/packages/DiAttributes

## Scoped, Transient, and Singleton

Classes can be registered as any of these three types of dependency via the respective attributes:

```cs
using DiAttributes;

[Scoped]
public class MyService
{ ... }
```

This is the equivalent of having the following in your `Startup.cs`:

```cs
services.Scoped<MyService>();
```

You can also pass in a type as an argument to register the class against:

```cs
using DiAttributes;

public interface IMyService
{ ... }

[Scoped(typeof(IMyService))]
public class MyService : IMyService
{ ... }
```

This is the equivalent of having the following in your `Startup.cs`:

```cs
services.Scoped<IMyService, MyService>();
```

The use of these attributes will require you to add the following line once to your `Startup.cs` file:

```cs
services.RegisterDiAttributes();
```

## Configuration

Classes can be automatically bound to sections of your app's configuration using the `[Configuration]` attribute.

If your `appsettings.json` looks like this:
```json
{
  "Outer": {
    "Inner": {
      "MySetting": "My Value"
    }
  }
}
```

Then you can bind a class to the `Inner` object (for example) and register it with the `IServiceCollection` like this:
```cs
using DiAttributes;

[Configuration("Outer:Inner")]
public class MyInnerOptions
{
  public string MySetting { get; set; }
}
```

To use this attribute you will need to pass an `ICollection` instance to the `RegisterDiAttributes` call in your `Startup.cs` file:
```cs
services.RegisterDiAttributes(Configuration);
```
## HttpClient

Classes can be registered as HttpClients using the `HttpClient` attribute.  
As per the [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0#typed-clients), these classes will be registered as transient.

```cs
using DiAttributes;

[HttpClient]
public class MyHttpClient
{
  public MyHttpClient(HttpClient httpClient)
  { ... }
}
```

This is the equivalent of having the following in your `Startup.cs`:

```cs
services.AddHttpClient<MyHttpClient>();
```

You can also pass in a type as an argument to register the class against:

```cs
public interface IMyHttpClient
{ ... }

[Scoped(typeof(IMyHttpClient))]
public class MyHttpClient : IMyHttpClient
{
  public MyHttpClient(HttpClient httpClient)
  { ... }
}
```

This is the equivalent of having the following in your `Startup.cs`:

```cs
services.AddHttpClient<IMyHttpClient, MyHttpClient>();
```
