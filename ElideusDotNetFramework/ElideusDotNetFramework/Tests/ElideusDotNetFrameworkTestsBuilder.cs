using ElideusDotNetFramework.Providers;
using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests.Mocks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkTestsBuilder
    {
        protected static bool _isInitialized { get; set; } = false;
        public static ElideusDotNetFrameworkTestsBuilder? Instance { get; private set; }
        public static IApplicationContext? ApplicationContextMock { get; private set; }

        protected virtual IMapperProvider MockAutoMapper()
        {
            var mapperProvider = new MapperProvider();

            mapperProvider.CreateMapper(new List<AutoMapper.Profile>());

            return mapperProvider;
        }

        protected virtual void ConfigureMocks()
        {
            if(!_isInitialized)
            {
                _isInitialized = true;

                //Mock application context
                ApplicationContextMock = new ApplicationContextMock().Mock();

                //Mock automapper
                ApplicationContextMock!.AddTestDependency(MockAutoMapper());
            }


        }
    }
}
