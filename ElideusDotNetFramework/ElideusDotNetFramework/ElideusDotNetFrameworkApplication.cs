using ElideusDotNetFramework.Providers.Contracts;
using ElideusDotNetFramework.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElideusDotNetFramework
{
    class ElideusDotNetFrameworkApplication
    {
        protected virtual bool UseAuthentication { get; set; } = false;

        protected virtual IServiceCollection InjectDependencies(ref WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMapperProvider, MapperProvider>();

            return builder.Services;
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

        protected virtual void MapOperations(ref WebApplication app)
        {

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

            var serviceCollection = this.InjectDependencies(ref builder);

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
