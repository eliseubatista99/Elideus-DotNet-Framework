using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests;
using ElideusDotNetFramework.Tests.Mocks;
using TechTalk.SpecFlow.xUnit.SpecFlowPlugin;

[assembly: AssemblyFixture(typeof(ElideusDotNetFrameworkTestsBuilder))]
namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder : IDisposable
    {
        public IApplicationContext? ApplicationContextMock { get; private set; }

        public ElideusDotNetFrameworkTestsBuilder()
        {
            ApplicationContextMock = new ApplicationContextMock().Mock();

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
