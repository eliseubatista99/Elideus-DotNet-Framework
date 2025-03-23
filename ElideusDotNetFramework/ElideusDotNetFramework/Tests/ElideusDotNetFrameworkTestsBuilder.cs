using ElideusDotNetFramework.Providers;
using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests.Mocks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder
    {
        protected static bool _initialized = false;
        protected static readonly object _lock = new object();

        public static IApplicationContext? applicationContext { get; private set; }

        protected virtual IMapperProvider MockAutoMapper()
        {
            var mapperProvider = new MapperProvider();

            mapperProvider.CreateMapper(new List<AutoMapper.Profile>());

            return mapperProvider;
        }

        protected virtual void ConfigureMocks()
        {
        }


        public void InitializeTests()
        {
            lock (_lock)
            {
                if (_initialized)
                {
                    return;
                }

                //Mock application context
                applicationContext = new TestsApplicationContext().Mock();

                //Mock automapper
                var autoMapper = MockAutoMapper();

                //Add automapper to application context dependencies
                applicationContext!.AddTestDependency<IMapperProvider>(new MapperProvider());

                ConfigureMocks();

                _initialized = true;
            }
        }

    }
}
