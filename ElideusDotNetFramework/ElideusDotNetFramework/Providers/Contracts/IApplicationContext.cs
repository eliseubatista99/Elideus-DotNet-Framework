using Microsoft.AspNetCore.Builder;

namespace ElideusDotNetFramework.Providers.Contracts
{
    public interface IApplicationContext
    {
        public void Initialize(ref WebApplicationBuilder _builder);

        public void AddDependency<TService, TImplementation>() where TService : class
            where TImplementation : class, TService;

        public T? GetDependency<T>() where T : class;
    }
}
