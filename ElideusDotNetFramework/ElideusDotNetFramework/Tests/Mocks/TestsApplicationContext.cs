using ElideusDotNetFramework.Providers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ElideusDotNetFramework.Tests.Mocks
{
    public class TestsApplicationContext : ElideusDotNetFrameworkTestsMock<IApplicationContext>, IApplicationContext
    {
        protected List<object> dependencies = new List<object>();

        protected virtual IConfiguration MockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"SectionName:SomeKey", "SectionValue"},
                //...populate as needed for the test
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return config;

        }

        protected virtual ILogger<IApplicationContext> MockLogger()
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());

            var logger = loggerFactory.CreateLogger<IApplicationContext>();

            return logger;
        }

        protected override IApplicationContext? ConfigureMock()
        {
            base.ConfigureMock();

            var configuration = MockConfiguration();
            var logger = MockLogger();

            AddTestDependency(configuration);
            AddTestDependency(logger);

            return this;
        }

        public T? GetDependency<T>() where T : class
        {
            if (dependencies.Count == 0)
            {
                return null;
            }

            foreach (var dependency in dependencies)
            {
                if (dependency is T)
                {
                    return (T)dependency;
                }
            }

            return default(T);
        }

        public void AddTestDependency<T>(T service)
        {
            dependencies.Add(service!);
        }

        void IApplicationContext.AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder)
        {
            //Do nothing
        }

        void IApplicationContext.AddDependency<TService, TImplementation>(ref WebApplicationBuilder builder, TImplementation implementation)
        {
            //Do nothing
        }
    }
}
