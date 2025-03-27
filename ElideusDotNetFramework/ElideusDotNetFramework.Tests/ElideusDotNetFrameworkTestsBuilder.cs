using ElideusDotNetFramework.Core;
using ElideusDotNetFramework.Tests;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow.xUnit.SpecFlowPlugin;

[assembly: AssemblyFixture(typeof(ElideusDotNetFrameworkTestsBuilder))]
namespace ElideusDotNetFramework.Tests
{
    [ExcludeFromCodeCoverage]
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
