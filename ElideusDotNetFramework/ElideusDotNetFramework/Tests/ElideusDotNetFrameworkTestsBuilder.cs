using ElideusDotNetFramework.Providers;
using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests.Mocks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder
    {
        protected static bool _initialized = false;
        protected static readonly object _lock = new object();

        protected virtual IMapperProvider MockAutoMapper()
        {
            var mapperProvider = new MapperProvider();

            mapperProvider.CreateMapper(new List<AutoMapper.Profile>());

            return mapperProvider;
        }

        protected virtual void ConfigureMocks(IApplicationContext applicationContext)
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
                var applicationContextMock = new TestsApplicationContext().Mock();

                //Mock automapper
                var autoMapper = MockAutoMapper();

                //Add automapper to application context dependencies
                applicationContextMock!.AddTestDependency<IMapperProvider>(new MapperProvider());

                ConfigureMocks(applicationContextMock!);

                _initialized = true;
            }
        }
    }
}
