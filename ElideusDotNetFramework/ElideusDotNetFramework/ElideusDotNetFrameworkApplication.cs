using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using ElideusDotNetFramework.Core.Operations;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Core
{
    [ExcludeFromCodeCoverage]
    public class ElideusDotNetFrameworkApplication
    {
        protected IApplicationContext? ApplicationContext { get; set; }
        protected virtual bool UseAuthentication { get; set; } = false;
        protected virtual OperationsBuilder OperationsBuilder { get; set; } = new OperationsBuilder();

        protected virtual void InjectDependencies(ref WebApplicationBuilder builder)
        {
            var mapper = new MapperProvider();
            mapper.CreateMapper(new List<AutoMapper.Profile>());

            ApplicationContext?.AddDependency<IMapperProvider, MapperProvider>(ref builder, mapper);
        }

        protected void InitializeApplicationContext(ref WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IApplicationContext, ApplicationContext>();
            ApplicationContext = builder.Services.BuildServiceProvider().GetService<IApplicationContext>()!;
        }

        protected virtual void ConfigureAuthentication(ref WebApplicationBuilder builder)
        {
            // Add the process of verifying what access they have
        }

        protected virtual void AddAuthorizationToSwagger(ref WebApplicationBuilder builder, ref SwaggerGenOptions options)
        {
            // Add the process of verifying what access they have
            builder.Services.AddAuthorization();
        }

        protected virtual void InitializeDatabase(ref WebApplicationBuilder builder)
        {

        }

        protected virtual void InitializeAutoMapper()
        {

        }

        protected void MapOperations(ref WebApplication app)
        {
            OperationsBuilder.MapOperations(ref app, ApplicationContext!);
        }



        public void InitializeApp(WebApplicationBuilder builder)
        {
            // Add Cors
            builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            this.InitializeApplicationContext(ref builder);
            this.InjectDependencies(ref builder);
            this.InitializeAutoMapper();

            if (UseAuthentication)
            {
                this.ConfigureAuthentication(ref builder);

                builder.Services.AddAuthorization();

            }

            InitializeDatabase(ref builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                if (UseAuthentication)
                {
                    AddAuthorizationToSwagger(ref builder, ref opt);
                }
            });

            var app = builder.Build();

            MapOperations(ref app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.SerializeAsV2 = true;
                });

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            if (UseAuthentication)
            {
                app.UseAuthentication();

                app.UseAuthorization();
            }

            app.Run();
        }
    }
}
