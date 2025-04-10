# Elideus | DotNet Framework External Services

**External Services** is a subpackage of the *Elideus-DotNet-Framework* that specifies the basic operations required to call an external service using `HttpClient`.

## Overview

This package includes a class called `ExternalServiceProvider` which implements `IExternalServiceProvider`. It defines the method `CallExternalPostOperation`, which handles serialization and the HTTP request process.


## Creating a Service Provider

Suppose we have an external application responsible for authentication. To interact with it, we define an interface:

```csharp
public interface IAuthAppProvider : IExternalServiceProvider
{
    public Task<AuthenticateOutput> Authenticate(AuthenticateInput input);
}
```

Now, we implement this interface in a class that inherits from `ExternalServiceProvider`.

In the constructor, instantiate the `HttpClient`:

```csharp
public AuthAppProvider(IApplicationContext _applicationContext) : base(_applicationContext)
{
    httpClient = new HttpClient();
}
```

Override the `GetServiceUrl` method to define the base URL of the external service:

```csharp
protected override string GetServiceUrl()
{
    return "https://url-to-ou-service.com";
}
```

Finally, use the `CallExternalPostOperation` method to perform the request. Specify the endpoint and the input/output types:

```csharp
public class AuthAppProvider : ExternalServiceProvider, IAuthAppProvider
{
    public Task<AuthenticateOutput> Authenticate(AuthenticateInput input)
    {
        return CallExternalPostOperation<AuthenticateInput, AuthenticateOutput>("Authenticate", input);
    }
}
```

## Using the Provider

Once you inject this dependency as described in the [Core documentation](https://github.com/eliseubatista99/Elideus-DotNet-Framework/blob/feat/documentation/ElideusDotNetFramework/ElideusDotNetFramework.Core/README.md),  
you can use it in your operations like any other dependency.

This enables seamless interaction with external APIs while keeping your codebase clean and structured.