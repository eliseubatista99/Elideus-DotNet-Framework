using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Core
{
    [ExcludeFromCodeCoverage]
    public class ApplicationContext : IApplicationContext
    {
        private static WebApplicationBuilder? applicationBuilder;

        public void AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder) where TService : class
            where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService, TImplementation>();
            applicationBuilder = builder;
        }

        public void AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder, TImplementation implementation) where TService : class
            where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService>(implementation);
            applicationBuilder = builder;
        }

        public void AddTestDependency<T>(T service)
        {
            //Do nothing
        }


        public T? GetDependency<T>() where T : class
        {
            return applicationBuilder!.Services.BuildServiceProvider().GetService<T>()!;
        }
    }
}
