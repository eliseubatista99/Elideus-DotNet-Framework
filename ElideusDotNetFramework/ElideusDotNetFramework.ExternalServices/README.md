# Elideus | DotNet Framework External Services

External Services is a subpackage of the Elideus-DotNet-Framework, and specifies the basic operations to call an external service with httpClient.

This package includes a class called ExternalServiceProvider which implements the IExternalServiceProvider, which defines an method called CallExternalPostOperation. 
It handles the serialization and the http request.

Lets say we have another app that we need to interact with that handles authentication for our app.
We define the IAuthAppProvider that extends IExternalServiceProvider, and has one method:

```csharp
public interface IAuthAppProvider : IExternalServiceProvider
{
	public Task<AuthenticateOutput> Authenticate(AuthenticateInput input);
}
```

The implementation for this provider, also extends the ExternalServiceProvider class. 
And there, in the constructor, we create an instance of the httpClient:

```csharp
public AuthAppProvider(IApplicationContext _applicationContext) : base(_applicationContext)
{
	httpClient = new HttpClient();
}
```

Then, to define the url of outr external service, we override the GetServiceUrl method, and there we return the base url of our service:

```csharp
protected override string GetServiceUrl()
{
	return "https://url-to-ou-service.com";
}
```

Now we can implement the AuthenticateOperation that uses the base class CallExternalPostOperation to make the request. All we need to do is specify the endpoint, and the input and output types.

```csharp
public interface AuthAppProvider : ExternalServiceProvider, IAuthAppProvider
{
	public Task<AuthenticateOutput> Authenticate(AuthenticateInput input)
	{
		return CallExternalPostOperation<AuthenticateInput, AuthenticateOutput>("Authenticate", input);
	}
}
```

If we inject this dependency as shown in the [core documentation](https://github.com/eliseubatista99/Elideus-DotNet-Framework/blob/feat/documentation/ElideusDotNetFramework/ElideusDotNetFramework.Core/README.md),
we can now use it in our operations.