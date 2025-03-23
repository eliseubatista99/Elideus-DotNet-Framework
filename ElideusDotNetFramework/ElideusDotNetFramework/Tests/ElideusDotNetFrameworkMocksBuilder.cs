using ElideusDotNetFramework.Providers;
using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Tests.Mocks;

namespace ElideusDotNetFramework.Tests
{
    public class ElideusDotNetFrameworkMocksBuilder
    {
        public virtual IMapperProvider MockAutoMapper()
        {
            var mapperProvider = new MapperProvider();

            mapperProvider.CreateMapper(new List<AutoMapper.Profile>());

            return mapperProvider;
        }

        public virtual void ConfigureRemainingMocks(IApplicationContext applicationContext)
        {
        }
    }
}
