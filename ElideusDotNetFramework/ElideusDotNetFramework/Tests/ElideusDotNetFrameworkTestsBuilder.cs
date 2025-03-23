using ElideusDotNetFramework.Providers;
using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests.Mocks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder
    {
        protected static readonly object _lock = new object();
        public static ElideusDotNetFrameworkTestsBuilder? Instance { get; private set; }
        public IApplicationContext? ApplicationContextMock { get; private set; }
        protected virtual ElideusDotNetFrameworkMocksBuilder? MocksBuilder { get; set; } = new ElideusDotNetFrameworkMocksBuilder();

        public static ElideusDotNetFrameworkTestsBuilder InitializeBuilder()
        {
            lock (_lock)
            {
                if (Instance == null)
                {
                    Instance = new ElideusDotNetFrameworkTestsBuilder();

                    //Mock application context
                    Instance.ApplicationContextMock = new ApplicationContextMock().Mock();

                    //Mock automapper
                    var autoMapper = Instance.MocksBuilder!.MockAutoMapper();

                    //Add automapper to application context dependencies
                    Instance.ApplicationContextMock!.AddTestDependency<IMapperProvider>(new MapperProvider());

                    Instance.MocksBuilder.ConfigureRemainingMocks(Instance.ApplicationContextMock);
                }

                return Instance;
            }

        }
    }
}
