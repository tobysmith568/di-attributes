# DiAttributes

Super-small and super-simple library for registering classes with the ASP.NET Core `IServiceCollection` using attributes.

GitHub: https://github.com/tobysmith568/di-attributes
NuGet: https://www.nuget.org/packages/DiAttributes/

## Scoped, Transient, and Singleton

Classes can be registered as any of these three types of dependency via the respective attributes:

```cs
[Scoped]
public class MyService
{
  
}
```

This is the equivilent of having the following in your `Startup.cs`:

```cs
services.Scoped<MyService>();
```

You can also pass in a type as an argument to register the class against:

```cs
public interface IMyService
{
  
}

[Scoped(typeof(IMyService))]
public class MyService : IMyService
{
  
}
```

This is the equivilent of having the following in your `Startup.cs`:

```cs
services.Scoped<IMyService, MyService>();
```

The use of these attributes will require you to add the following line to your `Startup.cs` file:
```cs
services.RegisterDiAttributes();
```

## Configuration

Classes can be automatically bound to sections of your app's configuration using the `Configuration` attribute.

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

Then you can bind a class to the `Inner` object and register it with the `IServiceCollection` like this:
```cs
[Configuration("Outer:Inner")]
public class MyInnerOptions
{
  public string MySetting { get; set; }
}
```

To use this attribute you will require need to pass an `ICollection` to the `RegisterDiAttributes` call in your `Startup.cs` file:
```cs
services.RegisterDiAttributes(Configuration);
```
