# Elideus | DotNet Framework Tests

Tests is a subpackage of the Elideus-DotNet-Framework, and it sets up the boilerplate for testing a project developed with the Elideus-DotNet-Framework.

### Setup

In order to start, we create a builder that implements the ElideusDotNetFrameworkTestsBuilder:

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

This builder has an annotation on top of the namespace that tells the xunit that this builder is to be shared across tests of the same assembly.
This way, only one instance of the builder is created per assembly. The main use of the builder class is to initialize dependencies and mocks.

One thing we have to mock, is the applicationContext. The application context mock is essential, since it contains the configurations from app settings and the mapper profiles.

The tests package offers an ApplicationContextMock, that handles the basics of mocking the application context. We only need to worry about
overriding the Configurations and MapperProfiles variables.

```csharp
public class MyAppContextMock : ApplicationContextMock
{
	protected override Dictionary<string, string?> Configurations { get; set; } = new Dictionary<string, string?>
	{
		{$"{Database}:{ConnectionString}", 1234connectionstring5678},
	};

	protected override List<Profile> MapperProfiles { get; set; } = new List<Profile>
	{
		new UserMapperProfile(),
	};
}
```

Now in the builder, we override the initialize method, and set the ApplicationContextMock.

```csharp
protected override void Initialize()
{
	base.Initialize();

	ApplicationContextMock = new MyAppContextMock().Mock();
}
```

We can also mock dependencies in the initialize method. Lets say we have a service responsible for the authentication, we can mock the response of
that service in order for it to always be sucess, because we don't need to test external services, that's their own responsability.

To do that, we create a mock class that implements the service interface:

```csharp
public class MyAuthDependencyMock : IMyAuthDependency
    {
        public async bool Autheticate(MyMethodInput input)
        {
            return true;
        }
    }
}
```

Now, in the initialize method, after we set the application context mock, we can do:


```csharp
ApplicationContextMock!.AddTestDependency(new MyAuthDependencyMock());
```

Now, everytime the IMyAuthDependency is used, this mock will be the implementation.

### Tests

In order to create a test, we create a class that inherits from OperationTest, for which we need to specify the Operation, Input and Output types. 
In the constructor, we can acess the builder inject by the AssemblyFixture of xunit. In the constructor, we instantiate the application to test.

```csharp
public class GetUsersOperationTests : OperationTest<GetUsersOperation, GetUsersInput, GetUsersOutput>
{
    public GetUsersOperationTests(MyAppTestsBuilder _testBuilder) : base(_testBuilder)
    {
	    OperationToTest = new GetUsersOperation(_testBuilder.ApplicationContextMock!, string.Empty);
    }
}
```

The OperationTest class also provides one method called SimulateOperationToTestCall, which calls the operation and parses the output for the operation type.
We can use that method in a test in a really simple way:

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