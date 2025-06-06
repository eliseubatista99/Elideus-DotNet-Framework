﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElideusDotNetFramework.Core.Operations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ElideusDotNetFramework.Core
{
    [ExcludeFromCodeCoverage]
    public class ElideusDotNetFrameworkApplication
    {
        protected IApplicationContext? ApplicationContext { get; set; }
        protected virtual OperationsBuilder OperationsBuilder { get; set; } = new OperationsBuilder();
        protected virtual bool UseAuthentication { get; set; } = true;

        protected virtual void InjectDependencies(ref WebApplicationBuilder builder)
        {
            var mapper = new MapperProvider();
            mapper.CreateMapper(new List<AutoMapper.Profile>());

            ApplicationContext?.AddDependency<IMapperProvider, MapperProvider>(ref builder, mapper);
            ApplicationContext?.AddDependency<IAuthenticationProvider, AuthenticationProvider>(ref builder);
        }

        protected void InitializeApplicationContext(ref WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IApplicationContext, ApplicationContext>();
            ApplicationContext = builder.Services.BuildServiceProvider().GetService<IApplicationContext>()!;
        }

        protected virtual void ConfigureAuthentication(ref WebApplicationBuilder builder)
        {
            var authProvider = ApplicationContext!.GetDependency<IAuthenticationProvider>()!;

            authProvider!.AddValidationParameters(ref builder);
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
            }

            if (UseAuthentication)
            {
                builder.Services.AddAuthorization();
            }

            InitializeDatabase(ref builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();;

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[0]
                        }
                    });
            });

            var app = builder.Build();

            app.UseCors();

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
