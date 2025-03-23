using ElideusDotNetFramework.Providers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ElideusDotNetFramework.Tests.Mocks
{
    public class ApplicationContextMock : TestMock<IApplicationContext>, IApplicationContext
    {
        protected virtual Dictionary<string, string?> Configurations { get; set; } = new Dictionary<string, string?>
        {
            {"SectionName:SomeKey", "SectionValue"},
        };

        protected List<object> dependencies = new List<object>();



        protected IConfiguration? MockConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(Configurations)
                .Build();

            return config;
        }

        protected ILogger? MockLogger()
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
