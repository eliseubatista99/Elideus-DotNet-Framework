# Elideus | DotNet Framework Core

**Core** is a subpackage of the *Elideus-DotNet-Framework*, providing the fundamental features for building an application.

To configure an application to use this package, you need to create an `ApplicationClass` that inherits from `ElideusDotNetFrameworkApplication`:

```csharp
public class MyApp : ElideusDotNetFrameworkApplication {

}
```

In the `Program.cs` file, you can replace all existing code with:

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

At this point, the application is ready to run, but without any actual logic or endpoints configured.

---

## Dependencies

In this framework, dependencies are injected into an `ApplicationContext` within the application class.

In the `MyApp` class we just created, you can override a method called `InjectDependencies`. In that method, you can use a property inherited from `ElideusDotNetFrameworkApplication` to inject dependencies into the application context.

Example:

```csharp
protected override void InjectDependencies(ref WebApplicationBuilder builder)
{
    base.InjectDependencies(ref builder);

    ApplicationContext?.AddDependency<IMyDatabaseProvider, MyDatabaseProvider>(ref builder);
}
```

All injected dependencies will then be available anywhere the `ApplicationContext` is accessible.

---

## Mapping

The Core package includes AutoMapper by default, exposing a dependency called `IMapperProvider`. This can be used to perform mapping operations and configure mapper profiles.

To configure mapper profiles, you can override the `InitializeAutoMapper` method in the `MyApp` class and add all necessary profiles:

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

To perform a mapping between types:

```csharp
var mapperProvider = executionContext.GetDependency<IMapperProvider>();
var mappedResult = mapperProvider.Map<Type1, Type2>(objectToMap);
```

---

## Operations

To expose endpoints, you must create an **operation**. An operation defines the logic behind a specific endpoint.

Let’s say we want to create an operation to retrieve a list of users.

First, define the input and output classes.

The input must inherit from `OperationInput` to be recognized by the framework:

```csharp
public class MyOperationInput : OperationInput
{
}
```

In this case, since we don’t need any parameters, we can use the built-in `VoidOperationInput`.

Similarly, the output must inherit from `OperationOutput`, which provides basic response structure:

```csharp
public class GetUsersOutput : OperationOutput
{
    public List<User> users { get; set; }
}
```

Now, create the `GetUsersOperation` that inherits from `BaseOperation`.

There are a few requirements when inheriting from the base operation. The constructor must receive the `ApplicationContext` and the endpoint path. Also, you must specify the input and output types:

```csharp
public class GetUsersOperation(IApplicationContext context, string endpoint): BaseOperation<VoidOperationInput, GetUsersOutput>
{

}
```

An operation can override three main methods:

### `InitAsync`

Executed as soon as the operation is called. Useful for retrieving dependencies:

```csharp
protected override async Task InitAsync()
{
    await base.InitAsync();

    mapperProvider = executionContext.GetDependency<IMapperProvider>()!;
}
```

### `ValidateInput`

Used to validate the input and return an error if invalid:

```csharp
protected override async Task<(HttpStatusCode? StatusCode, Error? Error)> ValidateInput(HttpRequest request, VoidOperationInput input)
{
    // Custom validation
}
```

### `ExecuteAsync`

The main operation logic. Returns the output object:

```csharp
protected override async Task<GetUsersOutput> ExecuteAsync(VoidOperationInput input)
{
    // Operation logic

    return new GetUsersOutput
    {
        // Data
    };
}
```

---

## Operations Builder

Once the operation is created, it needs to be mapped.

Operations are mapped using a class that inherits from `OperationsBuilder`:

```csharp
public class MyOperationsBuilder : OperationsBuilder
{

}
```

To tell the application which operations builder to use, override the `OperationsBuilder` property in your `MyApp` class:

```csharp
protected override OperationsBuilder OperationsBuilder { get; set; } = new MyOperationsBuilder();
```

Inside your builder class, override the `MapOperations` method. Use `MapPostOperation` to define each operation mapping.

This method requires the operation type, input and output types, and instances of the app, context, and operation.

Example:

```csharp
public override void MapOperations(ref WebApplication app, IApplicationContext context)
{
    MapPostOperation<GetUsersOperation, VoidOperationInput, GetUsersOutput>(ref app, context, new GetUsersOperation(context, "/GetUsers"));
}
```