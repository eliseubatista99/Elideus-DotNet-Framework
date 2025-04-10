# Elideus | DotNet Framework Tests

**Tests** is a subpackage of the *Elideus-DotNet-Framework*, and it sets up the boilerplate for testing a project developed with the framework.

---

## Setup

To start, create a builder that implements the `ElideusDotNetFrameworkTestsBuilder`:

```csharp
[assembly: AssemblyFixture(typeof(MyAppTestsBuilder))]
namespace MyApp.Tests
{
    public class MyAppTestsBuilder : ElideusDotNetFrameworkTestsBuilder
    {
        public MyAppTestsBuilder() : base()
        {
        }  
    }
}
```

This builder has an annotation that tells xUnit to share it across all tests in the same assembly. Only one instance of the builder will be created. The builder’s main role is to initialize dependencies and mocks.

---

## Application Context Mock

You need to mock the `ApplicationContext`, which holds app configurations and AutoMapper profiles.

The tests package provides `ApplicationContextMock`, which handles basic mocking. You just need to override the `Configurations` and `MapperProfiles` properties:

```csharp
public class MyAppContextMock : ApplicationContextMock
{
    protected override Dictionary<string, string?> Configurations { get; set; } = new Dictionary<string, string?>
    {
        { $"{Database}:{ConnectionString}", "1234connectionstring5678" },
    };

    protected override List<Profile> MapperProfiles { get; set; } = new List<Profile>
    {
        new UserMapperProfile(),
    };
}
```

Now override `Initialize` in the builder to apply this mock:

```csharp
protected override void Initialize()
{
    base.Initialize();

    ApplicationContextMock = new MyAppContextMock().Mock();
}
```

---

## Mocking Dependencies

You can also mock dependencies in the `Initialize` method. For example, you can mock an authentication service:

```csharp
public class MyAuthDependencyMock : IMyAuthDependency
{
    public async Task<bool> Authenticate(MyMethodInput input)
    {
        return true;
    }
}
```

And then register it like this:

```csharp
ApplicationContextMock!.AddTestDependency(new MyAuthDependencyMock());
```

Now, `IMyAuthDependency` will always resolve to your mock implementation.

---

## Writing Tests

To write a test, create a class that inherits from `OperationTest`. You need to specify the Operation, Input, and Output types.

In the constructor, use the `ApplicationContextMock` from your test builder and instantiate the operation to test:

```csharp
public class GetUsersOperationTests : OperationTest<GetUsersOperation, GetUsersInput, GetUsersOutput>
{
    public GetUsersOperationTests(MyAppTestsBuilder _testBuilder) : base(_testBuilder)
    {
        OperationToTest = new GetUsersOperation(_testBuilder.ApplicationContextMock!, string.Empty);
    }
}
```

Use the `SimulateOperationToTestCall` method to call the operation in a test:

```csharp
[Fact]
public async Task ShouldBe_Success()
{
    var response = await SimulateOperationToTestCall(new GetUsersInput
    {
        OnlyAdminUsers = true,
    });

    Assert.True(response.Error == null);
}
```