using ElideusDotNetFramework.Providers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ElideusDotNetFramework.Providers
{
    public class ApplicationContext : IApplicationContext
    {
        private WebApplicationBuilder? builder;

        public void Initialize(ref WebApplicationBuilder _builder)
        {
            builder = _builder;
        }

        public void AddDependency<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            builder?.Services.AddSingleton<TService, TImplementation>();
        }

        public T? GetDependency<T>() where T : class
        {
            return builder?.Services.BuildServiceProvider().GetService<T>();
        }
    }
}
