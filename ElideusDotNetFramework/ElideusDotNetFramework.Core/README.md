# Elideus | DotNet Framework Core

Core is a subpackage of the Elideus-DotNet-Framework, and contains the basic functionalities for an application.

In order to use configure an application to use this package, you need to create an ApplicationClass that inherits from the ElideusDotNetFrameworkApplication.

```csharp
public class MyApp : ElideusDotNetFrameworkApplication {

    }
```

And in the Program.cs file, you can remove and replace everything with:

```csharp
class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = new AppClass();

        app.InitializeApp(builder);
    }
}
```

This will have the application ready to start, but with nothing happening.

# Dependencies

In this framework, dependencies are inject to an ApplicationContext in the Application class.

In the MyApp class that we just created, we can override a method called inject dependencies.
In that method, we can use a variable inherited from the ElideusDotNetFrameworkApplication class to inject the dependencies in the application context.

An example would be:

```csharp
protected override void InjectDependencies(ref WebApplicationBuilder builder)
{
	base.InjectDependencies(ref builder);

	ApplicationContext?.AddDependency<IMyDatabaseProvider, MyDatabaseProvider>(ref builder);
}
```

Now, all our dependencies would be accessible everywhere where we have acess to the ApplicationContext.

# Mapping

The Core package include AutoMapper by default, offering a dependency called IMapperProvider. This dependency can be used to execute the Map functionality, and to configure the mapper profiles.

To assign the mapper profiles, we can override a method in the MyApp class called InitializeAutoMapper, there, we can add all the mapper profiles we need:

```csharp
protected override void InitializeAutoMapper()
{
    base.InitializeAutoMapper();

    var mapper = ApplicationContext.GetDependency<IMapperProvider>();

    mapper.CreateMapper(
        new List<AutoMapper.Profile>
        {
            new MyMapperProfileForThis(),
            new MyMapperProfileForThat(),
        });
}
```

Now, when we want to map two types, we can do something like:

```csharp
var mapperProvider = executionContext.GetDependency<IMapperProvider>();
var mappedResult = mapperProvider.Map<Type1, Type2>(objectToMap);
```

# Operations

To map some endpoints, we have to create an operation. An operation is the logic associated with one endpoint.

Lets imagine that we want to implement one operation to retrieve the list of users.

First, we need to define the input and output of the operation.

The input must inherit the OperationInput class, in order to be recognized by the operations.

```csharp
public class MyOperationInput : OperationInput
{
}
```

In this case, we want to get all the users, and we don't need anything in the input. The framework offers a class called VoidOperationInput that can be used for inputs with nothing.

In the same way, the output must inherit from the OperationOutput class, that includes some basic output information:

```csharp
public class GetUsersOutput : OperationOutput
{
    public List<User> users { get; set; }
}
```

Now create the GetUsersOperation that inherits from the BaseOperation.

There are somethings that are needed in the base operation in order to automatize all the boilerplate. First, we need to receive in the constructor the ApplicationContext dependency, and the endpoint name. We also need to tell the BaseOperation class the input and output types:

```csharp
public class GetUsersOperation(IApplicationContext context, string endpoint): BaseOperation<VoidOperationInput, GetUsersOutput>{

}
```

The operation offers 3 methods that can be overriden:

- InitAsync: Executes as soon as the operation is called, can be used to retrieve dependencies from the execution context:

```csharp
protected override async Task InitAsync()
{
    await base.InitAsync();

    mapperProvider = executionContext.GetDependency<IMapperProvider>()!;
}
```

- ValidateInput: Can be used to validate the operation input and throw errors if its not valid:

```csharp
protected override async Task<(HttpStatusCode? StatusCode, Error? Error)> ValidateInput(HttpRequest request, VoidOperationInput input)
{
	//Execute custom validation
}
```

- ExecuteAsync: This is the operation execution logic, and returns an object of the specified operation output type

```csharp
protected override async Task<GetUsersOutput> ExecuteAsync(VoidOperationInput input)
{
    //Execute the operation logic

    return new GetUsersOutput
    {
		//Data
    };
}
```

# Operations Builder

Now that we have an operation, we can start mapping it.

The operations mapping is done in the operations builder class. An operations builder class must inherit from the OperationsBuilder class.

```csharp
public class MyOperationsBuilder : OperationsBuilder
{

}
```

And now, we need to tell the application that this is the operations builder we want to use. To do that, in MyApp class we override the applications builder variable:

```csharp
protected override OperationsBuilder OperationsBuilder { get; set; } = new MyOperationsBuilder();

```

Now in our builder, we can override the map operations method. Inside this method, we can map the operations using the MapPostOperation methods inherited from the OperationsBuilder class.

This mapping needs us to specify the operation type, the input and output, and also, we need to pass as argument the app, the context, and an instance of the operation.

Lets see an example using our GetUsersOperation:

```csharp
public override void MapOperations(ref WebApplication app, IApplicationContext context)
{
	MapPostOperation<GetUsersOperation, VoidOperationInput, GetUsersOutput>(ref app, context, new GetUsersOperation(context, "/GetUsers"));
}

```
