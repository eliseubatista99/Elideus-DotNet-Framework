using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElideusDotNetFramework.Operations;

namespace ElideusDotNetFramework
{
    class ElideusDotNetFrameworkApplication
    {
        protected IApplicationContext? ApplicationContext { get; set; }
        protected virtual bool UseAuthentication { get; set; } = false;
        protected virtual OperationsBuilder OperationsBuilder { get; set; } = new OperationsBuilder();

        protected virtual void InjectDependencies(ref WebApplicationBuilder builder)
        {
            ApplicationContext?.AddDependency<IMapperProvider, MapperProvider>();
        }

        protected void InitializeApplicationContext(ref WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IApplicationContext, ApplicationContext>();
            ApplicationContext = builder.Services.BuildServiceProvider().GetService<IApplicationContext>()!;

            ApplicationContext.Initialize(ref builder);
        }

        protected virtual void ConfigureAuthentication(ref WebApplicationBuilder builder)
        {
            // Add the process of verifying what access they have
            builder.Services.AddAuthorization();
        }

        protected virtual void AddAuthorizationToSwagger(ref WebApplicationBuilder builder)
        {
            // Add the process of verifying what access they have
            builder.Services.AddAuthorization();
        }

        protected virtual void InitializeDatabase(ref WebApplicationBuilder builder)
        {

        }

        protected void MapOperations(ref WebApplicationBuilder builder, ref WebApplication app)
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

            if (UseAuthentication)
            {
                this.ConfigureAuthentication(ref builder);
            }

            InitializeDatabase(ref builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            if (UseAuthentication)
            {
                AddAuthorizationToSwagger(ref builder);
            }

            var app = builder.Build();

            MapOperations(ref builder,ref app);

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
