using AutoMapper;
using ElideusDotNetFramework.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ElideusDotNetFramework.Tests
{
    public class ApplicationContextMock : TestMock<IApplicationContext>, IApplicationContext
    {
        protected List<object> dependencies = new List<object>();

        protected virtual Dictionary<string, string?> Configurations { get; set; } = new Dictionary<string, string?>
        {
            {"SectionName:SomeKey", "SectionValue"},
        };

        protected virtual List<Profile> MapperProfiles { get; set; } = new List<Profile>
        {

        };

        protected IConfiguration? MockConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(Configurations)
                .Build();

            return config;
        }

        protected IMapperProvider MockAutoMapper()
        {
            var mapperProvider = new MapperProvider();

            mapperProvider.CreateMapper(MapperProfiles);

            return mapperProvider;
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
            var mapper = MockAutoMapper();

            AddTestDependency(configuration);
            AddTestDependency(logger);
            AddTestDependency(mapper);


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
