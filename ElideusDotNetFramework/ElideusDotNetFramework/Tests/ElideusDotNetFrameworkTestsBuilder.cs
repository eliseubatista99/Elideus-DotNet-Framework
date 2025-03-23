using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests;
using TechTalk.SpecFlow.xUnit.SpecFlowPlugin;

[assembly: AssemblyFixture(typeof(ElideusDotNetFrameworkTestsBuilder))]
namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder : IDisposable
    {
        public IApplicationContext? ApplicationContextMock { get; protected set; }

        public ElideusDotNetFrameworkTestsBuilder()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {

        }

        public void Dispose()
        {
            //Executes after all tests are done
        }
    }
}
