using Microsoft.AspNetCore.Builder;

namespace ElideusDotNetFramework.Core
{
    public interface IApplicationContext
    {
        public void AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder) where TService : class
            where TImplementation : class, TService;

        public void AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder, TImplementation implementation) where TService : class
            where TImplementation : class, TService;

        public void AddTestDependency<T>(T service);

        public T? GetDependency<T>() where T : class;
    }
}
